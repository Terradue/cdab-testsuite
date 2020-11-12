

pipeline {
    agent any
    stages {
        stage('Build CDAB client') {
            agent { 
                docker { 
                    image 'mono:6.8' 
                } 
            }
            environment {
                HOME = '$WORKSPACE'
            }
            steps {
                dir("src/cdab-client") {
                    echo 'Build CDAB client .NET application'
                    sh 'msbuild /t:build /Restore:true /p:Configuration=DEBUG'
                    stash includes: 'bin/**,App_Data/**,cdab-client', name: 'cdab-client-build'
                }
            }
        }
        stage('Package CDAB Client') {
            agent { 
                docker { 
                    image 'alectolytic/rpmbuilder:centos-7' 
                } 
            }
            steps {
                dir("src/cdab-client") {
                    unstash name: 'cdab-client-build'
                    sh 'mkdir -p build/{BUILD,RPMS,SOURCES,SPECS,SRPMS}'
                    sh 'cp cdab-client.spec build/SPECS/cdab-client.spec'
                    sh 'spectool -g -R --directory build/SOURCES build/SPECS/cdab-client.spec'
                    sh 'cp -r bin build/SOURCES/'
                    sh 'cp -r App_Data build/SOURCES/'
                    sh 'cp cdab-client build/SOURCES/'
                    script {
                        def sdf = sh(returnStdout: true, script: 'date -u +%Y%m%dT%H%M%S').trim()
                        if (env.BRANCH_NAME == 'master') {
                            env.release_cdab_client = env.BUILD_NUMBER
                        }
                        else {
                            env.release_cdab_client = 'SNAPSHOT' + sdf
                        }
                    }
                    echo 'Build package'
                    sh "rpmbuild --define \"_topdir ${pwd()}/build\" -ba --define '_branch ${env.BRANCH_NAME}' --define '_release ${env.release_cdab_client}' build/SPECS/cdab-client.spec"
                    sh "rpm -qpl ${pwd()}/build/RPMS/*/*.rpm"
                }
                stash includes: 'src/cdab-client/build/RPMS/**/*.rpm', name: 'cdab-client-rpm'
            }
        }
        stage('Package CDAB Remote Client') {
            agent { 
                docker { 
                    image 'alectolytic/rpmbuilder:centos-7' 
                } 
            }
            steps {
                dir("src/cdab-remote-client") {
                    sh 'mkdir -p build/{BUILD,RPMS,SOURCES,SPECS,SRPMS}'
                    sh 'cp cdab-remote-client.spec build/SPECS/cdab-remote-client.spec'
                    sh 'spectool -g -R --directory build/SOURCES build/SPECS/cdab-remote-client.spec'
                    sh 'cp -r bin build/SOURCES/'
                    sh 'cp -r libexec build/SOURCES/'
                    sh 'cp -r etc build/SOURCES/'
                    script {
                        def sdf = sh(returnStdout: true, script: 'date -u +%Y%m%dT%H%M%S').trim()
                        if (env.BRANCH_NAME == 'master') {
                            env.release_cdab_remote_client = env.BUILD_NUMBER
                        }
                        else {
                            env.release_cdab_remote_client = 'SNAPSHOT' + sdf
                        }
                    }
                    echo 'Build package'
                    sh "rpmbuild --define \"_topdir ${pwd()}/build\" -ba --define '_branch ${env.BRANCH_NAME}' --define '_release ${env.release_cdab_remote_client}' build/SPECS/cdab-remote-client.spec"
                    sh "rpm -qpl ${pwd()}/build/RPMS/*/*.rpm"
                }
                stash includes: 'src/cdab-remote-client/build/RPMS/**/*.rpm', name: 'cdab-remote-client-rpm'
            }
        }
        stage('Publish RPMs') {
            steps {
                unstash name: 'cdab-client-rpm'
                unstash name: 'cdab-remote-client-rpm'
                archiveArtifacts artifacts: 'src/*/build/RPMS/**/*.rpm', fingerprint: true
            }
        }

        stage('Build & Publish Docker') {
            steps {
                unstash name: 'cdab-client-rpm'
                unstash name: 'cdab-remote-client-rpm'
                sh "ls src/*/build/RPMS/*/"
                sh "mv src/cdab-client/build/RPMS/noarch/cdab-client-*.rpm src/docker/"
                sh "mv src/cdab-remote-client/build/RPMS/noarch/cdab-remote-client-*.rpm src/docker/"
                script {
                    def cdabclientrpm = findFiles(glob: "src/docker/cdab-client-*.rpm")
                    def cdabremoteclientrpm = findFiles(glob: "src/docker/cdab-remote-client-*.rpm")
                    def descriptor = readDescriptor()
                    def mType=getTypeOfVersion(env.BRANCH_NAME)
                    def testsuite = docker.build(descriptor.docker_image_name, "--build-arg CDAB_RELEASE=${mType}${descriptor.version} --build-arg CDAB_CLIENT_RPM=${cdabclientrpm[0].name} --build-arg CDAB_REMOTE_CLIENT_RPM=${cdabremoteclientrpm[0].name} ./src/docker")
                    docker.withRegistry('https://registry.hub.docker.com', 'dockerhub-emmanuelmathot') {
                      testsuite.push("${mType}${descriptor.version}")
                      testsuite.push("${mType}latest")
                    }
                }
            }
        }
    }
}

def getTypeOfVersion(branchName) {
  
  def matcher = (env.BRANCH_NAME =~ /master/)
  if (matcher.matches())
    return ""
  
  return "dev"
}

def readDescriptor (){
    return readYaml(file: 'build.yml')
}