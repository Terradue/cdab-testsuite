In this page, we will run the Test Scenario #1 testing the Target Site availability with a ping, query Simple data and download single data. We assume you have followed the first part of this guide and the [Test Site Preparation](Home#test-site-preparation)

We are going to run some Test Scenarios with 2 Target Sites:

* [Test Scenario TS01](Test-Scenarios-Description#ts01) on [Copernicus Open Access Hub](https://scihub.copernicus.eu/)
* [Test Scenario TS02](Test-Scenarios-Description#ts02) on [Copernicus Open Access Hub](https://scihub.copernicus.eu/)
* [Test Scenario TS05](Test-Scenarios-Description#ts05) on [Mundi Web Services](https://mundiwebservices.com)
* [Test Scenario TS13](Test-Scenarios-Description#ts13) on [Mundi Web Services](https://mundiwebservices.com)

> 💡 All Test Scenarios and their Test Cases are described in the [Test Scenarios Description](Test-Scenarios-Description) page.

The Test Scenarios are run via 2 different command line tools according to the type of Test Tcenario:

* [**Local Test Scenarios**](Test-Scenarios-Description#Local-Test-Scenarios) are  locally executed and ran using **cdab-client** command line tool.
* [**Remote Test Scenarios**](Test-Scenarios-Description#Remote-Test-Scenarios) are remotely executed on a virtual machines within the service providers' cloud infrastructure and ran using **cdab-remote-client** command line tool.

## Start the Test Site Container

We will start the Test Site docker container we will use for this benchmark session.

```console
docker run --detach --name testsite-1 esacdab/testsuite:latest
```

Your Test Site Container should be ready. You can control it with the following command and yoiu should have a similar output

```
$ docker ps

CONTAINER ID        IMAGE                                               COMMAND                  CREATED              STATUS              PORTS                    NAMES
0a7c0ab615fe        esacdab/testsuite:latest                            "/usr/bin/supervisor…"   About a minute ago   Up About a minute   22/tcp                   testsite-1
```

Let's now copy the `config.yaml` file [previously prepared](Home#prepare-the-test-suite-configuration-configyaml) to the container

```console
docker cp config.yaml testsite-1:config.yaml
```

For the processing scenario running on a virtual machine in the MUNDI cloud, we also have to copy the private key file for the SSH access and the docker configuration file for the download of the docker image that is used for the processing.

```console
docker cp cdab-key-mundi.pem testsite-1:cdab-key-mundi.pem
docker cp config.yaml testsite-1:config.yaml
```

## Run Query & Download Test Scenarios

In this first benchmark, we will run the 2 first Test Scenarios [TS01](Test-Scenarios-Description#ts01) and [TS02](Test-Scenarios-Description#ts02) on [Copernicus Open Access Hub](https://scihub.copernicus.eu/).

The first one will benchmark basic data access functions with several catalog saearches and then a single download:

```console
docker exec -it testsite-1 cdab-client -v -tsn=testsite-1 -tn=SciHub TS01
```

The Test will begin and quickly you console will output something similar to

```console
2020-11-11 13:22:04,645 [1] INFO  cdabtesttools.MainClass - ===============================
2020-11-11 13:22:04,658 [1] INFO  cdabtesttools.MainClass - Validating Test Options...
2020-11-11 13:22:04,658 [1] INFO  cdabtesttools.MainClass - [1] Loading & Checking configuration
2020-11-11 13:22:04,777 [1] DEBUG cdabtesttools.MainClass - Configuration found in /config.yaml
2020-11-11 13:22:04,778 [1] INFO  cdabtesttools.MainClass - [2] Configuring target SciHub (https://scihub.copernicus.eu/dhus)
2020-11-11 13:22:04,791 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Scan /usr/lib/cdab-client/bin for OpenSearch plugins
2020-11-11 13:22:04,801 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Found GeoJson [GeoJson native query] in class FeatureCollectionOpenSearchEngineExtension
2020-11-11 13:22:04,801 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Registering extension: json
2020-11-11 13:22:04,804 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Found Atom [Atom native query] in class AtomOpenSearchEngineExtension
2020-11-11 13:22:04,804 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Registering extension: atom
2020-11-11 13:22:04,928 [1] DEBUG Terradue.OpenSearch.DataHub.DHuS.DataHubWrapperSettings - DHUS OpenSearch API identified
2020-11-11 13:22:04,930 [1] DEBUG cdabtesttools.Target.TargetSiteWrapper - TARGET TYPE: DATAHUB
2020-11-11 13:22:04,930 [1] INFO  cdabtesttools.MainClass - [3] Check scenarios compatibility...
2020-11-11 13:22:04,932 [1] INFO  cdabtesttools.MainClass - 1 Test Scenarios : TS01
2020-11-11 13:22:04,932 [1] INFO  cdabtesttools.MainClass - Test Options VALIDATED
2020-11-11 13:22:04,932 [1] INFO  cdabtesttools.MainClass - ===============================
2020-11-11 13:22:04,933 [1] INFO  cdabtesttools.MainClass - Setting ThreadPool Min/Max : 8/800
2020-11-11 13:22:04,948 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 13:22:04,948 [1] INFO  cdabtesttools.MainClass - *   EXECUTING TEST SCENARIOS   *
2020-11-11 13:22:04,948 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 13:22:04,949 [1] INFO  cdabtesttools.MainClass - Creating Test Cases for Test Scenario [TS01] Simple data search and single download
2020-11-11 13:22:07,672 [1] INFO  cdabtesttools.MainClass - 3 Test Case(s) for Test Scenario [TS01]
2020-11-11 13:22:07,672 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC101] Service Reachability
2020-11-11 13:22:07,673 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC201] Basic catalogue query
2020-11-11 13:22:07,673 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC301] Single Remote Download
2020-11-11 13:22:07,673 [Thread Pool Worker] INFO  cdabtesttools.MainClass - Tests Ignition ...
2020-11-11 13:22:07,673 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 3
2020-11-11 13:22:08,673 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 2
2020-11-11 13:22:09,674 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 1
2020-11-11 13:22:10,674 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 0
2020-11-11 13:22:11,674 [Thread Pool Worker] INFO  cdabtesttools.MainClass - Tests FIRED !!!
2020-11-11 13:22:11,675 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Prepare TC101
2020-11-11 13:22:11,725 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - > HTTP HEAD https://scihub.copernicus.eu/dhus ...
2020-11-11 13:22:11,725 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - > HTTP HEAD https://scihub.copernicus.eu/dhus ...
2020-11-11 13:22:11,762 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Connected to scihub.copernicus.eu
2020-11-11 13:22:11,762 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Connected to scihub.copernicus.eu
2020-11-11 13:22:12,636 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Reply from scihub.copernicus.eu
2020-11-11 13:22:12,638 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - SP CC 1/7
2020-11-11 13:22:12,640 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - < HTTP/1.1 200 OK 874ms
2020-11-11 13:22:13,022 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Reply from scihub.copernicus.eu
2020-11-11 13:22:13,022 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - SP CC 1/7
2020-11-11 13:22:13,022 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - < HTTP/1.1 200 OK 1260ms
2020-11-11 13:22:13,064 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Average Response Time : 1067ms
2020-11-11 13:22:13,065 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Peak Response Time : 1260ms
2020-11-11 13:22:13,065 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Error Rate : 0%
2020-11-11 13:22:13,066 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - average Concurrency : 1.68#
2020-11-11 13:22:13,066 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - peak Concurrency : 2#
2020-11-11 13:22:13,067 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Prepare TC201
2020-11-11 13:22:13,088 [Thread Pool Worker] DEBUG Terradue.OpenSearch.DataHub.DHuS.DataHubWrapperSettings - DHUS OpenSearch API identified
...
```

Test Scenario [TS01](Test-Scenarios-Description#ts01) should be executed quite quickly and once completed, you should see the last lines like this:

```console

```

We will now copy the benchmark results of this Test Scenario run.

```console
docker cp testsite-1:TS01Results.json testsite-1-TS01-SciHub-results.json
```

The file contains all the metrics for each [Test Case](Test-Scenarios-Description#test-cases) measured during the benchmark.

> 💡 The Test Results document produced by the tools are described in the[Test Results Description](Test-Results-Description) page.

Let's now start the second Test Scenario [TS02](Test-Scenarios-Description#ts02)

This first one will benchmark more advanced data access function with several catalog saearches and then multiple parallel downloads:

```console
docker exec -it testsite-1 cdab-client -v -tsn=testsite-1 -tn=SciHub TS02
```

Again the output of the console will show the progress of the Test Case

```console
2020-11-11 13:41:03,540 [1] INFO  cdabtesttools.MainClass - ===============================
2020-11-11 13:41:03,554 [1] INFO  cdabtesttools.MainClass - Validating Test Options...
2020-11-11 13:41:03,554 [1] INFO  cdabtesttools.MainClass - [1] Loading & Checking configuration
2020-11-11 13:41:03,683 [1] DEBUG cdabtesttools.MainClass - Configuration found in /config.yaml
2020-11-11 13:41:03,683 [1] INFO  cdabtesttools.MainClass - [2] Configuring target SciHub (https://scihub.copernicus.eu/dhus)
2020-11-11 13:41:03,697 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Scan /usr/lib/cdab-client/bin for OpenSearch plugins
2020-11-11 13:41:03,706 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Found GeoJson [GeoJson native query] in class FeatureCollectionOpenSearchEngineExtension
2020-11-11 13:41:03,707 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Registering extension: json
2020-11-11 13:41:03,709 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Found Atom [Atom native query] in class AtomOpenSearchEngineExtension
2020-11-11 13:41:03,709 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Registering extension: atom
2020-11-11 13:41:03,845 [1] DEBUG Terradue.OpenSearch.DataHub.DHuS.DataHubWrapperSettings - DHUS OpenSearch API identified
2020-11-11 13:41:03,848 [1] DEBUG cdabtesttools.Target.TargetSiteWrapper - TARGET TYPE: DATAHUB
2020-11-11 13:41:03,848 [1] INFO  cdabtesttools.MainClass - [3] Check scenarios compatibility...
2020-11-11 13:41:03,850 [1] INFO  cdabtesttools.MainClass - 1 Test Scenarios : TS02
2020-11-11 13:41:03,850 [1] INFO  cdabtesttools.MainClass - Test Options VALIDATED
2020-11-11 13:41:03,850 [1] INFO  cdabtesttools.MainClass - ===============================
2020-11-11 13:41:03,851 [1] INFO  cdabtesttools.MainClass - Setting ThreadPool Min/Max : 8/800
2020-11-11 13:41:03,869 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 13:41:03,869 [1] INFO  cdabtesttools.MainClass - *   EXECUTING TEST SCENARIOS   *
2020-11-11 13:41:03,869 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 13:41:03,870 [1] INFO  cdabtesttools.MainClass - Creating Test Cases for Test Scenario [TS02] Complex data search and bulk download
2020-11-11 13:41:06,669 [1] INFO  cdabtesttools.MainClass - 2 Test Case(s) for Test Scenario [TS02]
2020-11-11 13:41:06,669 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC202] Complex Query
2020-11-11 13:41:06,670 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC302] Multiple Remote Download
2020-11-11 13:41:06,670 [Thread Pool Worker] INFO  cdabtesttools.MainClass - Tests Ignition ...
2020-11-11 13:41:06,670 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 3
2020-11-11 13:41:07,670 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 2
2020-11-11 13:41:08,671 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 1
2020-11-11 13:41:09,671 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 0
2020-11-11 13:41:10,671 [Thread Pool Worker] INFO  cdabtesttools.MainClass - Tests FIRED !!!
2020-11-11 13:42:06,514 [Thread Pool Worker] DEBUG Terradue.OpenSearch.DataHub.DHuS.DataHubWrapperSettings - DHUS OpenSearch API identified
2020-11-11 13:42:06,516 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - [1] > Query Complex-Random-0 Sentinel-2 S2MSI1C intersecting Switzerland...
2020-11-11 13:42:06,993 [Thread Pool Worker] DEBUG Terradue.OpenSearch.DataHub.DHuS.DHuSOpenSearchClient - COMMAND URL : https://scihub.copernicus.eu:443/dhus/search?rows=20&q=footprint%3a%22Intersects(POLYGON+((6.2+46.4%2c+7+47.5%2c+8.5+47.8%2c+10.5+46.9%2c+9+45.8%2c+8.5+46.5%2c+7.9+45.9%2c+6.2+46.4)))%22+AND+producttype%3aS2MSI1C+AND+platformname%3aSentinel-2
2020-11-11 13:42:06,993 [Thread Pool Worker] DEBUG Terradue.OpenSearch.DataHub.DHuS.DHuSOpenSearchClient - Querying (try = 3)...
...
```

After some minutes, once completed...

```console
2020-11-11 13:51:55,497 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - [6] 617.3MB downloaded (file #1:98.0% / total:98.1%) [2.2MB/s]
2020-11-11 13:51:57,140 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - [6] 623.6MB downloaded (file #1:99.0% / total:99.1%) [2.2MB/s]
2020-11-11 13:51:58,713 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Average Response Time : 314ms
2020-11-11 13:51:58,713 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Peak Response Time : 320ms
2020-11-11 13:51:58,713 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Error Rate : 0%
2020-11-11 13:51:58,713 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - average Concurrency : 1.62#
2020-11-11 13:51:58,713 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - peak Concurrency : 2#
2020-11-11 13:51:58,714 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Max Size : 660220058bytes
2020-11-11 13:51:58,715 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Total Size : 968718082bytes
2020-11-11 13:51:58,716 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Total Throughput : 3383920bytes/second
2020-11-11 13:51:58,716 [Thread Pool Worker] DEBUG cdabtesttools.Measurement.MeasurementsAnalyzer - Data Collection Division : S2A_MSIL1C_20150730T072206_N0204_R106_T39RVH_20150730T072203 S2A MSI S2MSI1C Level-1C starting at 07/30/2015 07:22:06 ending at 07/30/2015 07:22:06,S2A_MSIL1C_20150730T072206_N0204_R106_T39RVJ_20150730T072203 S2A MSI S2MSI1C Level-1C starting at 07/30/2015 07:22:06 ending at 07/30/2015 07:22:06
2020-11-11 13:51:58,870 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 13:51:58,870 [1] INFO  cdabtesttools.MainClass - *   COMPLETED TEST SCENARIOS   *
2020-11-11 13:51:58,870 [1] INFO  cdabtesttools.MainClass - ********************************
```

... we copy the results

```console
docker cp testsite-1:TS02Results.json testsite-1-TS02-SciHub-results.json
```

## Run Cataloge Coverage Test Scenario

In this section, we will run the Test Scenarios [TS05](Test-Scenarios-Description#ts05) on [Mundi Web Services](https://mundiwebservices.com).

This Test Scenario will measure the coverage of the Target Site catalogue in comparison of the reference catalogue on [Copernicus Open Access Hub](https://scihub.copernicus.eu/)

```console
docker exec -it testsite-1 cdab-client -v -tsn=testsite-1 -tn=MUNDI TS05
```

the output of the console will show the progress of the Test Cases

```console
2020-11-11 14:01:07,770 [1] INFO  cdabtesttools.MainClass - ===============================
2020-11-11 14:01:07,784 [1] INFO  cdabtesttools.MainClass - Validating Test Options...
2020-11-11 14:01:07,784 [1] INFO  cdabtesttools.MainClass - [1] Loading & Checking configuration
2020-11-11 14:01:07,909 [1] DEBUG cdabtesttools.MainClass - Configuration found in /config.yaml
2020-11-11 14:01:07,909 [1] INFO  cdabtesttools.MainClass - [2] Configuring target MUNDI (https://mundiwebservices.com/acdc/catalog/proxy/search)
2020-11-11 14:01:07,924 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Scan /usr/lib/cdab-client/bin for OpenSearch plugins
2020-11-11 14:01:07,935 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Found GeoJson [GeoJson native query] in class FeatureCollectionOpenSearchEngineExtension
2020-11-11 14:01:07,936 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Registering extension: json
2020-11-11 14:01:07,938 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Found Atom [Atom native query] in class AtomOpenSearchEngineExtension
2020-11-11 14:01:07,938 [1] DEBUG Terradue.OpenSearch.Engine.OpenSearchEngine - Registering extension: atom
2020-11-11 14:01:08,067 [1] DEBUG cdabtesttools.Target.TargetSiteWrapper - TARGET TYPE: DIAS
2020-11-11 14:01:08,067 [1] INFO  cdabtesttools.MainClass - [3] Check scenarios compatibility...
2020-11-11 14:01:08,069 [1] INFO  cdabtesttools.MainClass - 1 Test Scenarios : TS05
2020-11-11 14:01:08,070 [1] INFO  cdabtesttools.MainClass - Test Options VALIDATED
2020-11-11 14:01:08,070 [1] INFO  cdabtesttools.MainClass - ===============================
2020-11-11 14:01:08,070 [1] INFO  cdabtesttools.MainClass - Setting ThreadPool Min/Max : 8/800
2020-11-11 14:01:08,087 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 14:01:08,087 [1] INFO  cdabtesttools.MainClass - *   EXECUTING TEST SCENARIOS   *
2020-11-11 14:01:08,087 [1] INFO  cdabtesttools.MainClass - ********************************
2020-11-11 14:01:08,087 [1] INFO  cdabtesttools.MainClass - Creating Test Cases for Test Scenario [TS05] Coverage Analysis
2020-11-11 14:01:08,090 [1] INFO  cdabtesttools.MainClass - 3 Test Case(s) for Test Scenario [TS05]
2020-11-11 14:01:08,090 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC501] Catalogue Coverage
2020-11-11 14:01:08,091 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC502] Target Local Data Coverage
2020-11-11 14:01:08,091 [1] INFO  cdabtesttools.MainClass - Queuing Tasks for Test Case [TC503] Target Local Data Offer Consistency
2020-11-11 14:01:08,092 [Thread Pool Worker] INFO  cdabtesttools.MainClass - Tests Ignition ...
2020-11-11 14:01:08,092 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 3
2020-11-11 14:01:09,092 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 2
2020-11-11 14:01:10,093 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 1
2020-11-11 14:01:11,093 [Thread Pool Worker] INFO  cdabtesttools.MainClass - 0
2020-11-11 14:01:12,094 [Thread Pool Worker] INFO  cdabtesttools.MainClass - Tests FIRED !!!
2020-11-11 14:01:12,098 [Thread Pool Worker] DEBUG cdabtesttools.MainClass - Prepare TC501
...
```

After some minutes, once completed...

```console

```

... we copy the results

```console
docker cp testsite-1:TS05Results.json testsite-1-TS05-Mundi-results.json
```

## Run Cloud services Processing Test Scenario

In this section, we will run the Test Scenarios [TS13](Test-Scenarios-Description#ts13) on [Mundi Web Services](https://mundiwebservices.com).

This Test Scenario will measure the computing performance of a typical Earth Observation Processing Algorithm on the Target Site cloud infrastructure. Most of the load shall be done on the virtual machine started on the remote infrastructure. The local container will simply wait for the completion and compile the metrics.

```console
docker exec -it testsite-1 cdab-remote-client -v -conf=config.yaml -v -vm=1 -sp=MUNDI -ts=MUNDI -n=MUNDI TS13
```

The output of the console will show the progress of the Test Cases:

```console
...
2020-11-06T15:33:16.331845Z [INFO]  cdab-remote-client version 1.28
2020-11-06T15:33:16.331875Z [INFO]  Checking for old resources to delete ...
2020-11-06T15:33:16.332142Z [DEBUG] Command: openstack server list -f json --os-auth-url https://iam.eu-de.otc.t-systems.com:443/v3 ...
2020-11-06T15:33:19.522224Z [DEBUG] Command: openstack volume list -f json --os-auth-url https://iam.eu-de.otc.t-systems.com:443/v3 ...
2020-11-06T15:33:22.744039Z [INFO]  Done
2020-11-06T15:33:22.744171Z [INFO]  Obtaining list of available floating IP addresses ...
2020-11-06T15:33:22.744480Z [DEBUG] Command: openstack floating\ ip list -f json --os-auth-url https://iam.eu-de.otc.t-systems.com:443/v3 ...
2020-11-06T15:33:25.131797Z [INFO]  Available floating IP addresses: 80.158.58.122, 80.158.5.179, 80.158.7.134, 80.158.44.63, 80.158.47.173, 80.158.2.136
2020-11-06T15:33:25.132001Z [INFO]  Start of execution
2020-11-06T15:33:25.132699Z [INFO]  Creating virtual machine ...
2020-11-06T15:33:25.133040Z [DEBUG] Command: openstack server create --wait -f json --os-auth-url https://iam.eu-de.otc.t-systems.com:443/v3 ... cdab-test-mundi-48ca4482
2020-11-06T15:33:48.463922Z [DEBUG] {...}
2020-11-06T15:33:48.464096Z [INFO]  Virtual machine '506586c1-64f5-406b-8033-4673e22b6bb6' created
2020-11-06T15:33:48.464145Z [INFO]  Assigning floating IP address ...
2020-11-06T15:33:48.464360Z [DEBUG] Command: openstack server add floating\ ip --os-auth-url https://iam.eu-de.otc.t-systems.com:443/v3 ... 506586c1-64f5-406b-8033-4673e22b6bb6 80.158.58.122
2020-11-06T15:33:53.511692Z [INFO]  IP address 80.158.58.122 assigned explicitly
2020-11-06T15:33:53.511854Z [INFO]  Awaiting SSH availability ...
2020-11-06T15:33:53.512372Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "ls"
...
2020-11-06T15:34:44.568503Z [INFO]  Virtual machine available
...
2020-11-06T15:35:08.762503Z [INFO]  Installing and starting docker ...
2020-11-06T15:35:08.762794Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "sudo yum install -y yum-utils device-mapper-persistent-data lvm2"
2020-11-06T15:35:12.930048Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "sudo yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo"
2020-11-06T15:35:14.201981Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "sudo yum install docker-ce docker-ce-cli containerd.io -y"
2020-11-06T15:35:55.694565Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "sudo systemctl start docker"
...
2020-11-06T15:36:06.749615Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "sudo usermod -a -G docker $USER"
2020-11-06T15:36:07.664600Z [INFO]  Docker service started
2020-11-06T15:36:07.665044Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "mkdir .docker"
2020-11-06T15:36:08.546214Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... docker-config.json linux@80.158.58.122:.docker/config.json
2020-11-06T15:36:09.552809Z [INFO]  Installing docker image for test ...
2020-11-06T15:36:09.553309Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "docker pull esacdab/geohazards-tep/ewf-s3-olci-composites:0.41"
2020-11-06T15:51:17.480898Z [INFO]  Docker image installed
2020-11-06T15:51:19.364119Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... config-mundi.yaml linux@80.158.58.122:config.yaml
2020-11-06T15:51:20.460051Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... ts-scripts/s3-olci-composites.py linux@80.158.58.122:./s3-olci-composites.py
2020-11-06T15:51:21.539316Z [INFO]  Running processing test scenario TS13 ...
2020-11-06T15:51:21.539754Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... ts-scripts/TS13-remote.sh linux@80.158.58.122:TS13-remote.sh
2020-11-06T15:51:22.558491Z [DEBUG] Command: ssh -i cdab-key-mundi.pem ... linux@80.158.58.122 "sh TS13-remote.sh . \"esacdab/geohazards-tep/ewf-s3-olci-composites:0.41\" MUNDI MUNDI ..."


```

This processing may take some time, due to the large size of the docker image it uses. After a while, once completed, the output will continue as follows:

```console
2020-11-06T16:04:04.919408Z [INFO]  Test completed
2020-11-06T16:04:04.921933Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... linux@80.158.58.122:/mnt/cdab-volume/test/TS13Results.json TestResult-remote-48ca4482.json 
2020-11-06T16:04:05.947749Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... linux@80.158.58.122:/mnt/cdab-volume/test/junit.xml junit-remote--48ca4482.xml 
2020-11-06T16:04:06.920356Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... linux@80.158.58.122:/mnt/cdab-volume/test/cdab.stdout cdab-48ca4482.stdout 
2020-11-06T16:04:07.897211Z [DEBUG] Command: scp -i cdab-key-mundi.pem ... linux@80.158.58.122:/mnt/cdab-volume/test/cdab.stderr cdab-48ca4482.stderr 
2020-11-06T16:04:08.888813Z [INFO]  Test result files received
2020-11-06T16:04:08.888995Z [INFO]  stdout and stderr from cdab-client execution on virtual machine below
2020-11-06T16:04:08.889055Z [INFO]  --------------------------------
2020-11-06T16:04:08.889084Z [INFO]  remote execution stdout (START)
...
('Executing processing graph\n....10%....20%....30%....40%....50%....60%....70%....80%....90% done.\n', 'INFO: org.esa.snap.core.gpf.operators.tooladapter.ToolAdapterIO: Initializing external tool adapters\nWARNING: org.esa.snap.core.util.EngineVersionCheckActivator: A new SNAP version is available for download.\nCurrently installed 6.0, available is 8.0.0.\nPlease visit http://step.esa.int\n\nINFO: org.hsqldb.persist.Logger: dataFileCache open start\nINFO: java.util.prefs.FileSystemPreferences$1: Created user preferences directory.\n')
Done.
k = S3 OLCI Natural Colors
BANDS = ['s3_olci.data/Oa08_reflectance.img', 's3_olci.data/Oa06_reflectance.img', 's3_olci.data/Oa04_reflectance.img', 's3_olci.data/pixel_classif_flags.img']
2020-11-06 16:03:29 (INFO): Default RAM limit for OTB is 128 MB
2020-11-06 16:03:29 (INFO): GDAL maximum cache size is 802 MB
2020-11-06 16:03:29 (INFO): OTB will use at most 4 threads
2020-11-06 16:03:29 (INFO): Estimated memory for full processing: 566.619MB (avail.: 128 MB), optimal image partitioning: 5 blocks
2020-11-06 16:03:29 (INFO): Estimation will be performed in 6 blocks of 2112x2112 pixels
2020-11-06T16:04:08.894477Z [INFO]  remote execution stdout (END)
2020-11-06T16:04:08.894572Z [INFO]  remote execution stderr (START)
...
WARNING:Fiona:PROJ data files not located, PROJ_LIB not set
EXIT CODE = 0
-rw-r--r--. 1 root root 30456665 Nov  6 16:04 S3-OLCI-NATURAL-COLORS-20201031T231058-20201031T231058.png
-rw-r--r--. 1 root root      891 Nov  6 16:04 S3-OLCI-NATURAL-COLORS-20201031T231058-20201031T231058.png.properties
-rw-r--r--. 1 root root 88060234 Nov  6 16:03 S3-OLCI-NATURAL-COLORS-20201031T231058-20201031T231058.tif
-rw-r--r--. 1 root root      879 Nov  6 16:03 S3-OLCI-NATURAL-COLORS-20201031T231058-20201031T231058.tif.properties
-rw-r--r--. 1 root root 88060234 Nov  6 16:03 S3-OLCI-NATURAL-COLORS-20201031T231058-20201031T231058.tif
2020-11-06T16:04:08.898047Z [INFO]  remote execution stderr (END)
2020-11-06T16:04:08.898149Z [INFO]  --------------------------------
...
2020-11-06T16:04:27.734130Z [INFO]  Deleting virtual machine '506586c1-64f5-406b-8033-4673e22b6bb6' ...
2020-11-06T16:04:27.734348Z [DEBUG] Command: openstack server delete ... 506586c1-64f5-406b-8033-4673e22b6bb6 
2020-11-06T16:04:30.956052Z [INFO]  Virtual machine deleted
2020-11-06T16:04:30.958353Z [INFO]  Test run finished
--------------------------------------------------------------------
Timing summary for Test run
* VM creation request:                   2020-11-06T15:33:25.132777Z
* VM ready to use:                       2020-11-06T15:34:44.568306Z
* Docker and image installation started: 2020-11-06T15:35:08.762622Z
* Test started:                          2020-11-06T15:51:17.481430Z
* Test finished:                         2020-11-06T16:04:04.918552Z
* Test results downloaded:               2020-11-06T16:04:08.888691Z
* VM deleted:                            2020-11-06T16:04:30.955966Z
--------------------------------------------------------------------
Obtained metrics
* Error rate (%):                        0.0
* Total duration (ms):                   1865823
* Average provisioning latency (ms):     79436
* Average concurrency (#):               1
* Peak concurrency (#):                  1
* Run 'Test run'
  - Cost per hour (EUR):                 0.0
  - Cost per month (EUR):                0.0
  - Duration (ms):                       1865823
  - Process duration (ms):               729043
  - Provisioning latency (ms):           79436
--------------------------------------------------------------------
2020-11-06T16:04:30.984282Z [INFO]  Output file written: TS13Results.json
2020-11-06T16:04:30.994425Z [DEBUG] Command: xmllint --format junit.xml.tmp 
2020-11-06T16:04:31.002329Z [INFO]  Output file written: junit.xml
2020-11-06T16:04:31.003467Z [INFO]  End of execution
```

... we copy the results

```console
docker cp testsite-1:TS13Results.json testsite-1-TS13-Mundi-results.json
```

## Stop and Clean the Test Site Container

At the end of the benchmark session, when all reaults have been copied, we can stop and remove the the container.

```console
docker stop testsite-1
docker rm testsite-1
```

> ⚠️ It is important to clean container after a benchmark session. Indeed, it contains its own image and all the data downloaded during the tests. Severall gigabytes of disk space may be used!
