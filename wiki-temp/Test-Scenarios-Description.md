The Test Suite is organized around a series of [Test Scenarios](#Test-Scenarios) chaining a set of [Test Cases](#Test-Cases).

## Test Scenarios

The **Test Scenarios** are designed as a sequence of basic [Test Cases](#Test-Cases) covering one or several functionalities of the Target Site in order to reproduce a typical user operation on the target site.

The following table describes each Test Scenario vided in 2 groups:

* [**Local Test Scenarios**](#Local-Test-Scenarios) are  locally executed measuring metrics of the various functions from the local Test Site towards the Target Site. They are run using the **cdab-client** command line tool.
* [**Remote Test Scenarios**](#Remote-Test-Scenarios) are remotely executed on a virtual machines within the service providers' cloud infrastructure (when available) measuring metrics of the various functions directy within the Target Site. They are run using the **cdab-remote-client** command line tool.


### Local Test Scenarios

Scenario ID | Title                                                                             | Test case sequence
-----------------------------|------------------------------------------------------------------|----------------------
<a id="ts01"></a>TS01        | Simple data search and single download                           | TC101 → TC201 → TC301
<a id="ts02"></a>TS02        | Complex data search and bulk download                            | TC101 → TC202 → TC302
<a id="ts03"></a>TS03        | Systematic periodic data search and related remote data download | TC203 → TC303
<a id="ts04"></a>TS04        | Offline data download                                            | TC204 → TC304
<a id="ts05"></a>TS05        | Data Coverage Analysis                                           | TC501 → TC502
<a id="ts06"></a>TS06        | Data Latency Analysis                                            | TC601 → TC602

### Remote Test Scenarios

Scenario ID           | Title | Test cases        |
--------------------------------------------------|-----------------------------------------------------------------------|------------------------
<a id="ts11"></a>TS11 | Cloud services simple local data search and single local download on single virtual machines      | TC411 (→ TC211 → TC311)
<a id="ts12"></a>TS12 | Cloud services complex local data search and multiple local download on multiple virtual machines | TC412 (→ TC212 → TC312)
<a id="ts13"></a>TS13 | Cloud services simple local data search, download and simple processing of downloaded data        | TC413



## Test Cases

The following sections give a short overview about the simple test cases and how they can be configured and run.


<a id="tc101"></a>
### TC101: Service Reachability

This test performs multiple concurrent remote HTTP web requests to the front endpoint of the target site. It measures, among other metrics, the average and peak response times.


<a id="tc201"></a>
### TC201: Basic Query

This test performs a simple filtered search (e.g. by mission or product type) and verfies whether the results match the specified search criteria. The test client sends multiple concurrent remote HTTP web requests to the front OpenSearch API of the target site using the OpenSearch mechanism to query and retrieve the search results. Searches are limited to simple filters (no spatial nor time filters) established randomly on the missions dictionary.

Among the obtained metrics are the average and peak response times, the number of results and the size of the responses.


<a id="tc202"></a>
### TC202: Complex Query (Geo-Time Filter)

This test performs a more complex filtered search (e.g. by geometry, acquisition period or ingestion date) and verifies whether the results match the specified search criteria.
The test client sends multiple concurrent remote HTTP web requests are sent to the front catalogue search API (preferably OpenSearch API) of the target site using the search mechanism to query and retrieve the search results.
N queries are prepared with all filters (spatial and time filters included) and composed with random filters from the missions dictionary.

The obtained metrics are the same as in TC201.


<a id="tc203"></a>
### TC203: Specific Query (Handle Multiple Results Pages)

This test performs a specific filtered search (e.g. geometry, acquisition period, ingestion date) with many results pages and verfies whether the results match the specified search criteria.
The test client sends multiple concurrent remote HTTP web requests to the front OpenSearch API of the target site using the OpenSearch mechanism to query and retrieve the search results over many results pages. The search filters are fixed (a moving window in time).

The obtained metrics are the same as in TC201.


<a id="tc204"></a>
### TC204: Offline Data Query

This test performs a simple filtered search (e.g. by mission or product type) for querying only offline data and verifies whether the results match the specified search criteria.
The test client sends multiple concurrent remote HTTP web requests are sent to the front catalogue search API (preferably OpenSearch API) of the target site using the search mechanism to query and retrieve the search results.
N queries are prepared with simple filters (no spatial nor time filters) plus a specific filter to select offline data only. They are composed with random filters from the missions dictionary.

The obtained metrics are the same as in TC201.


<a id="tc211"></a>
### TC211: Basic Query from Cloud Services

This test is the remote version of TC201. Its results are derived from executing TC201 from a virtual machine on the target provider's cloud (if the provider offers processing infrastructure).


<a id="tc212"></a>
### TC212: Complex Query from Cloud Services

This test is the remote version of TC202. Its results are derived from executing TC202 from a virtual machine on the target provider's cloud (if the provider offers processing infrastructure).


<a id="tc301"></a>
### TC301: Single Remote Online Download

This test evaluates the download service of the target site for online data.
The test client makes a single remote download request to retrieve a product file via a product URL.

Among the metrics the test obtains is the throughput of the downloaded data.


<a id="tc302"></a>
### TC302: Multiple Remote Online Download

This test evaluates the download capacity of the target site for online data using it maximum concurrent download capacity.
It is the same as TC301 with as many concurrent download as the configured maximum allows.

The obtained metrics are the same as in TC301.

<a id="tc303"></a>
### TC303: Remote Bulk Download

This test evaulates the download capacity of the target site for downloading data in bulk.
It is the same as TC301 with as many download as the systematic search (TC213) returned.

The obtained metrics are the same as in TC301.

<a id="tc304"></a>
### TC304: Offline Download

This test evaluates the capacity of the target site for downloading offline data.
The test client sends multiple concurrent remote HTTP web requests to retrieve one or several product files from a set of selected URLs that are pointing to offline data.

The obtained metrics are the same as in TC301 and also include the latency for the availability of offline products.

<a id="tc311"></a>
### TC311: Single Remote Online Download from Cloud Services

This test is the remote version of TC301. Its results are derived from executing TC301 from a virtual machine on the target provider's cloud (if the provider offers processing infrastructure).


<a id="tc312"></a>
### TC312: Multiple Remote Online Download from Cloud Services

This test is the remote version of TC302. Its results are derived from executing TC302 from a virtual machine on the target provider's cloud (if the provider offers processing infrastructure).


<a id="tc411"></a>
### TC411: Cloud Services Single Virtual Machine Provisioning

This test measures the cloud services capacity of the target site for provisioning a single virtual machine.
The test client sends a remote web request using the cloud services API of the target site to request a typical virtual machine.
Once the machine is ready, the test client executes a command within a docker container to start TC211 and TC311.

The obtained metrics include the provisioning latency and and the process duration as well as information about the virtual machine configuration and related costs.


<a id="tc412"></a>
### TC412: Cloud Services Multiple Virtual Machine Provisioning

This test measures the cloud services capacity of the target site for provisioning multiple virtual machines.
The test client sends remote web requests using the cloud services API of the target site to request N typical virtual machines. Once a machine is ready, the test client executes a command within a docker container to start TC212 and TC312.

The obtained metrics are the same as in TC411.


<a id="tc413"></a>
### TC413: Cloud Services Virtual Machine Provisioning for Processing

This test measures the cloud services capacity of the target site for provisioning virtual machines with the capability of running data-transforming algorithms.
The test client sends a remote web requests using the cloud services API of the target site to request a typical virtual machine. Once the machine is ready, the test client executes a command within a docker container to download a test product and run an algorithm to produce one or more outputs from it.

The obtained metrics are the same as in TC411.

<a id="tc501"></a>
### TC501: Catalogue Coverage

This test case evaluates the catalogue coverage of a target site by collection.
The test client sends multiple concurrent catalogue requests to retrieve the total number of products for all the possible combinations of filters in configuration input.
When timeliness is applicable on a collection, the search excludes the time critical items (e.g. NRT, STC)

The obtained metrics include information about the number of results and coverage percentage.


<a id="tc502"></a>
### TC502: Local Data Coverage

This test case evaluates the local data coverage of a target site for all product types collection.
The test client sends multiple concurrent catalogue requests to retrieve the total number of online and offline products for all the possible product types.

The obtained metrics are the same as in TC501.


<a id="tc503"></a>
### TC503: Data Offer Consistency

This test evaluates the local data consistency of a target site by data offer collection.
The test client sends multiple concurrent catalogue requests to retrieve the total number of online and offline products for all the possible product types.

The obtained metrics are the same as in TC501.


<a id="tc601"></a>
### TC601: Data Operational Latency Analysis [Time Critical]

This test evaluates the data latency of a target site by collection.
The test client sens multiple concurrent catalogue requests to retrieve the latest products per collection and compare their data publication time to the sensing time.
A timeliness is applied on a collection when applicable to limit the search to the time critical items (e.g. NRT, STC).

The obtained metrics include the average and maximum data operational latency and information about result quality.


<a id="tc602"></a>
### TC602: Data Availability Latency Analysis

This test evaluates the availability data latency of a target site by collection with respect to a reference target site.
The test client sends multiple concurrent catalogue requests to retrieve the latest products per collection and compare their data publication time to the sensing time.
When a timeliness is applicable on a collection, searches are excluding the time critical items (e.g. NRT, STC).

The obtained metrics include the average and maximum data availability latency and information about result quality.

