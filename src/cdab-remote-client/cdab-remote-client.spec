
%define debug_package %{nil}
%define __jar_repack  %{nil}
%define _python_bytecompile_errors_terminate_build 0

Name:           cdab-remote-client
Url:            https://git.terradue.com/systems/cdab-cli
License:        AGPLv3
Group:          Productivity/Networking/Web/Servers
Version:        1.22
Release:        %{_release}
Summary:        Copernicus Sentinels Data Access Worldwide Benchmark Test Remote Client
BuildArch:      noarch
Source:         /usr/bin/cdab-client
Requires:       bash, coreutils, centos-release-scl, rh-python36
AutoReqProv:    no

%description
Copernicus Sentinels Data Access Worldwide Benchmark Test Remote Client

%prep

%build


%install
mkdir -p %{buildroot}/var/opt/cdab-remote-client/bin
cp %{_sourcedir}/bin/cdab-remote-client %{buildroot}/var/opt/cdab-remote-client/bin/cdab-remote-client
mkdir -p %{buildroot}/var/opt/cdab-remote-client/libexec
cp -r %{_sourcedir}/libexec %{buildroot}/var/opt/cdab-remote-client/libexec
mkdir -p %{buildroot}/var/opt/cdab-remote-client/etc
cp -r %{_sourcedir}/etc %{buildroot}/var/opt/cdab-remote-client/etc


%post
SUCCESS=0

# Install OpenStack client, Google Cloud Platform Python API and Amazon AWS EC2 Python API
/opt/rh/rh-python36/root/usr/bin/pip install python-openstackclient==5.1.0
/opt/rh/rh-python36/root/usr/bin/pip install google-api-python-client boto3

# Add symlink to cdab-remote-client
ln -s /var/opt/cdab-remote-client/bin/cdab-remote-client /usr/bin/cdab-remote-client
[ ! -f /var/opt/cdab-remote-client/etc/config.yaml ] && cp /var/opt/cdab-remote-client/etc/config.yaml.sample /var/opt/cdab-remote-client/etc/config.yaml

/opt/rh/rh-python36/root/usr/bin/pip install --upgrade pip

exit ${SUCCESS}

%postun


%clean
rm -rf %{buildroot}


%files
/var/opt/cdab-remote-client/*

%changelog
