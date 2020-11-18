# Compute QoE indicators

In the following sub-sections, the QoE criteria are described in details, together with the approach used to compute them:

* [**Q1 Copernicus Collections Richness**](#Q1-Copernicus-Collections-Richness) - How rich is the Copernicus Sentinels data offer?
* [**Q2 Reactivity**](#Q2-Reactivity) - How reactive is the target site in responding to the user's request?
* [**Q3 Discovery**](#Q3-Discovery) - How efficently can users find (and visualize) the desired Sentinels products?
* [**Q4 Download**](#Q4-Download) - How quickly can users download the identified Sentinels product/s of interest?
* [**Q5 Cloud Computing**](#Q5-Cloud-Computing) - How efficient and cost-effective is the Sentinels data access and processing in the cloud?

In the following the example values are taken from tests executed from the benchmark service on a period of some weeks, so with an high value of sample counts.

The QoE value is classified with the following convention:
* A: value > 90 and <= 100
* B: value > 60 and <= 90
* C: value > 40 and <= 60
* D: value > 10 and <= 40
* E: value > 0  and <= 10

## Q1 Copernicus Collections Richness

This question addresses the richness of given Copernicus data collections (taken as the “reference collections”) offered by a given target site.
Copernicus Sentinels data are available in different configurations in the different portals, depending on the specific objective of the site. ESA's Open Hub offers the most complete collection of Copernicus Sentinels data, including all product types produced as part of the baseline currently agreed with the European Commission, globally, for the whole missions timespans.

To compute the **Copernicus Collections Richness** indicator the following parameters are taken into account:

* The **data coverage** (%), providing a measure of the completeness of the reference data collections hosted on a given target site. This is computed as the ratio between the number of “reference collections” relevant data products that are available at the target site and the corresponding number at the reference site.
* The **catalogue coverage** (%), providing a measure of the completeness of the reference collections indexed in the catalogue of the target site. This is computed as the ratio between the number of the “reference collections” relevant items in the catalogue of the target site and corresponding number at the reference site.
* The **data availability latency** (hours), providing a measure of how quickly the “reference collections” relevant data are available at the target site with respect to the reference site. This is computed as the average latency of the availability as compared to the date of publication on the reference site.
* The consistency with the public **data offer** (%), which is a measure of the extent to which the public advertisement on “data collections” made by the target site is actually honoured. This is measured as the dataCoverage parameter above but limited to the subsets of the “reference collections” that are equivalent to the ones of the target site’s public data offer (i.e. same timespan, same geographic coverage and same product types)

The following Test Scenarios needs to be run in order to compute the Q1 indicator:

### Q1 Test Scenarios

Scenario ID | Title | Test case sequence
----------------------|------------------------|----------------
TS05 | Data Coverage Analysis | TC501 -> TC502 -> TC503
TS06 | Data Latency Analysis  | TC601 -> TC602

then from the [Test Results](Test-Results-Description) file extract the following metrics:

QoE | Test Cases | Metric | uom | better | satisfaction |frustration | weigth
----|------------|--------|-----|--------|--------------|------------|-------
Q1 | TC502 | dataCoverage |	% | high | - | - | 0.50
Q1 | TC501 | catalogueCoverage | % | high | - | - | 0.15
Q1 | TC602 | avgDataAvailabilityLatency | sec | low | 21600 | 86400 | 0.15
Q1 | TC503 | dataOfferConsistency | % | high | 99 | 90 | 0.20

those metrics are stored as array split by collection (dataCollectionDivision):
* Compute the average value/100 for: **dataCoverage** and **catalogueCoverage**
* Compute the apdex value for **avgDataAvailabilityLatency** and **dataOfferConsistency** according to the frustration and satisaction thresholds defined above and the method described in [QoE Indicators Description](QoE-Indicators-Description).

For example in case of **scihub** target, after the computaton you will end up to a partial output like the following table:

Target Site | Apdex/Metric | weigth | Apdex/Metric Value | Satisfied Counts | Tolerating Counts | Total Counts
------------|--------------|--------|--------------------|------------------|-------------------|-------------
scihub | apdex_avgDataAvailabilityLatency | 0.15 | 1.000 | 15 | 0 | 15
scihub | catalogueCoverage	              | 0.15 | 1.000 | 20 | 0 | 20
scihub | dataCoverage	                  | 0.50 | 1.001 | 20 | 0 | 20
scihub | apdex_dataOfferConsistency	      | 0.20 | 1.000 | 15 | 0 | 15

the Q1 value for scihub will be the weigthed average of such schema leading to 1 => 100 % (A)


## Q2 Reactivity

This question relates to the overall responsiveness of the portal: when a user tries to connect, how quickly does the site respond? How frequently it is found unavailable (apart from known downtimes)? These performances can go unperceived if they keep within reasonable limits, but become extremely disturbing when low levels are hit.

The target site general reachability is evaluated by performing multiple concurrent remote HTTP web requests to the front endpoint of the target site that can be executed multiple times per day.

To compute the **Reactivity** indicator the following parameters are taken into account:

* The time needed for the site to respond to the user request (both **average and peak response times** are taken into account); and
* The associated **error rates** (i.e. when the site is not responding at all)

The following Test Scenarios needs to be run in order to compute the Q2 indicator:

### Q2 Test Scenarios

Scenario ID | Title | Test case sequence
----------------------|------------------------|----------------
TS01 | Simple data search and single download | TC101 -> TC201 -> TC301

then from the [Test Results](Test-Results-Description) file extract the following metrics:

QoE | Test Cases | Metric | uom | better | satisfaction |frustration | weigth
----|------------|--------|-----|--------|--------------|------------|-------
Q2 | TC101 | avgResponseTime  | ms | low | 500  |2000 | 0.40
Q2 | TC101 | peakResponseTime | ms | low | 1000 |4000 | 0.20
Q2 | TC101 | errorRate        | %  | low | 1    |10   | 0.40

* Compute the apdex value for all the above metrics according to the frustration and satisaction thresholds defined above and the method described in [QoE Indicators Description](QoE-Indicators-Description).

For example in case of **scihub** target, after the computaton you will end up to a partial output like the following table:

Target Site | Apdex/Metric | weigth | Apdex/Metric Value | Satisfied Counts | Tolerating Counts | Total Counts
------------|--------------|--------|--------------------|------------------|-------------------|-------------
scihub | apdex_avgResponseTime | 0.40 | 0.601 | 33  | 124 | 158
scihub | apdex_errorRate       | 0.40 | 1.000 | 158 |   0 | 158
scihub | apdex_peakResponseTime| 0.20 | 0.753 | 80  |  78 | 158

the Q2 value for scihub will be the weigthed average of such schema leading to 0.79 => 79 % (B)


## Q3 Discovery

The question is of utmost importance because the result of a query can strongly impact the user experience and in some cases drive the overall judgment on the site, e.g. when stringent timeliness requirements are at stake.

The site performances in this respect are evaluated through multiple concurrent remote HTTP web requests to the front (opensearch) API of the target site using different queries, from basic ones (e.g. by mission, by product type, etc.) to more complex ones (e.g. by geographic locations, time intervals, ingestion date, etc.)

To compute the **Discoverability** indicator the following parameters are taken into account:

* The time needed for the site to respond to the user request (both **average and peak response times** are taken into account); and
* The associated **error rates** (i.e. when the site is not responding at all)
* The **result error rate**, the ratio of results not corresponding to a search query filters

The following Test Scenarios needs to be run in order to compute the Q3 indicator:

### Q3 Test Scenarios

Scenario ID | Title | Test case sequence
----------------------|------------------------|----------------
TS01 | Simple data search and single download | TC101 -> TC201 -> TC301
TS02 | Complex data search and bulk download | TC202 -> TC302
TS03 | Systematic periodic data search and related remote data download | TC203 -> TC303
TS04 | Offline data download | TC204 -> TC304

then from the [Test Results](Test-Results-Description) file extract the following metrics:

QoE | Test Cases | Metric | uom | better | satisfaction |frustration | weigth
----|------------|--------|-----|--------|--------------|------------|-------
Q3 | TC201,TC202,TC203,TC204 | avgResponseTime  | ms | low |  5000 | 20000 | 0.30
Q3 | TC201,TC202,TC203,TC204 | peakResponseTime | ms | low | 10000 | 40000 | 0.10
Q3 | TC201,TC202,TC203,TC204 | errorRate        | %  | low |     1 |    10 | 0.30
Q3 | TC201,TC202,TC203,TC204 | resultsErrorRate | %  | low |     1 |    30 | 0.30

* Compute the apdex value for all the above metrics according to the frustration and satisaction thresholds defined above and the method described in [QoE Indicators Description](QoE-Indicators-Description).

For example in case of **scihub** target, after the computaton you will end up to a partial output like the following table:

Target Site | Apdex/Metric | weigth | Apdex/Metric Value | Satisfied Counts | Tolerating Counts | Total Counts
------------|--------------|--------|--------------------|------------------|-------------------|-------------
scihub | apdex_avgResponseTime  | 0.30 | 0.474 | 128 | 57   | 330
scihub | apdex_errorRate        | 0.30 | 0.620 | 204 | 35   | 357
scihub | apdex_peakResponseTime | 0.10 | 0.509 | 147 | 42   | 330
scihub | apdex_resultsErrorRate | 0.30 | 0.805 | 214 | 102  | 329

the Q3 value for scihub will be the weigthed average of such schema leading to 0.62 => 62 % (B)


## Q4 Download

This question is core to the data accessibility for any user wishing to use the data on their local infrastructure. We define download times as the elapsed time between the moment the request is submitted on the system and the one when the data is available at the user infrastructure.

The download performances are evaluated through single or multiple concurrent remote HTTP web requests to retrieve several product files from a set of selected URLs, where the URLs for download are obtained from the results of the search scenarios. Also bulk downloads are executed, over a pre-defined set of selected URLs.

To compute the **Download** indicator the following parameters are taken into account:

* The time needed for the site to respond to the user request (both **average and peak response times** are taken into account); and
* The associated **error rates** (i.e. when the site is not responding at all)
* The **throughput**, i.e. the speed at which data are being downloaded, computed as the ratio between the time it takes to download a file and its size
* The additional **offline availability latency** due to the fact that a product is stored off-line (where foreseen)

The following Test Scenarios needs to be run in order to compute the Q4 indicator:

### Q4 Test Scenarios

Scenario ID | Title | Test case sequence
----------------------|------------------------|----------------
TS01 | Simple data search and single download | TC101 -> TC201 -> TC301
TS02 | Complex data search and bulk download | TC202 -> TC302
TS03 | Systematic periodic data search and related remote data download | TC203 -> TC303
TS04 | Offline data download | TC204 -> TC304

then from the [Test Results](Test-Results-Description) file extract the following metrics:

QoE | Test Cases | Metric | uom | better | satisfaction |frustration | weigth
----|------------|--------|-----|--------|--------------|------------|-------
Q4 | TC301,TC302,TC303 | avgResponseTime     | ms           | low  | 500     | 2000    | 0.10
Q4 | TC301,TC302,TC303 | peakResponseTime    | ms           | low  | 1000    | 4000    | 0.05
Q4 | TC301,TC302,TC303 | errorRate           | %            | low  | 1       | 10      | 0.20
Q4 | TC301,TC302,TC303 | throughput          | bytes/second | high | 8000000 | 1000000 | 0.50
Q4 | TC304 | offlineDataAvailabilityLatency  | seconds      | low  | 3600    | 86400   | 0.15

* Compute the apdex value for all the above metrics according to the frustration and satisaction thresholds defined above and the method described in [QoE Indicators Description](QoE-Indicators-Description).

For example in case of **scihub** target, after the computaton you will end up to a partial output like the following table:

Target Site | Apdex/Metric | weigth | Apdex/Metric Value | Satisfied Counts | Tolerating Counts | Total Counts
------------|--------------|--------|--------------------|------------------|-------------------|-------------
scihub | apdex_avgResponseTime                 | 0.10 |0.964 | 271 | 19  | 291
scihub | apdex_errorRate                       | 0.20 |1.000 | 291 | 0   | 291
scihub | apdex_offlineDataAvailabilityLatency  | 0.15 |0.700 | 15  | 12  | 30
scihub | apdex_peakResponseTime                | 0.05 |0.995 | 288 | 3   | 291
scihub | apdex_throughput                      | 0.50 |0.713 | 146 | 123 | 291

the Q4 value for scihub will be the weigthed average of such schema leading to 0.81 => 81 % (B)


## Q5 Cloud Computing

This question is typically referred to the Data Access Information Service (DIAS) platforms, providing cloud environments to exploit Copernicus collections.

The use scenario is complex, because it encompasses several functionalities that range from the allocation of VMs (for a dynamic one) to the upload of user data to the processing.

To compute the **Cloud Processing** indicator the following parameters are taken into account:

* The time needed for provisioning one or more virtual machine in a cloud service provider (**average provisioning latency**) and
* The **Cost score** as relative indicator of VM processing cost.
  * This score is computed as: (max(TCcost) - TCcost)/(max(TCcost) - min(TCcost)) , where the TCcost is the sum of costHour(flavourName) x durationHour of each TestCase (TC) run. In the reference period under consideration the max(TCcost) and min(TCcost) values greater than zero are computed among all the Target and TestCase. The cost_score value will be a number between 0 and 1, where 1 is for minimum cost and 0 for the maximum paid TC.
* The **Discoverability score** (Q53). This score take into account the following parameters: 
  * The time needed for the site to respond to the user request (both **average and peak response times** are taken into account); and
  * The associated **error rates** (i.e. when the site is not responding at all)
  * The **result error rate**, the ratio of results not corresponding to a search query filters
* The **Download score** (Q54). This score take into account the following parameters: 
  * The time needed for the site to respond to the user request (both **average and peak response times** are taken into account); and
  * The associated **error rates** (i.e. when the site is not responding at all)
  * The **throughput**, i.e. the speed at which data are being downloaded, computed as the ratio between the time it takes to download a file and its size

The following Test Scenarios needs to be run in order to compute the Q5 indicator:

### Q5 Test Scenarios

Scenario ID | Title | Test cases 
----------------------|------------------------|----------------
TS11 | Cloud services simple local data search and single local download on single virtual machines | TC411 (-> TC211 -> TC311)
TS12 | Cloud services complex local data search and multiple local download on multiple virtual machines | TC412 (-> TC212 -> TC312)
TS13 | Cloud services simple local data search, download and simple processing of downloaded data | TC413

then from the [Test Results](Test-Results-Description) file extract the following metrics:

QoE | Test Cases | Metric | uom | better | satisfaction |frustration | weigth
----|------------|--------|-----|--------|--------------|------------|-------
Q5  | TC411,TC412,TC413 | avgProvisioningLatency  | ms           | low   | 60000   | 90000   | 0.10
Q5  | TC411,TC412,TC413 | cost_score              | %            | high  | -       | -       | 0.10
Q5  | -                 | Q53_score               | %            | high  | -       | -       | 0.40
Q5  | -                 | Q54_score               | %            | high  | -       | -       | 0.40
Q53 | TC4*,TC211,TC212  | avgResponseTime         | ms           | low   | 5000    | 20000   | 0.30
Q53 | TC4*,TC211,TC212  | peakResponseTime        | ms           | low   | 10000   | 40000   | 0.10
Q53 | TC4*,TC211,TC212  | errorRate               | %            | low   | 1       | 10      | 0.30
Q53 | TC4*,TC211,TC212  | resultsErrorRate        | %            | low   | 1       | 30      | 0.30
Q54 | TC4*,TC311,TC312  | avgResponseTime         | ms           | low   | 500     | 2000    | 0.20
Q54 | TC4*,TC311,TC312  | peakResponseTime        | ms           | low   | 1000    | 4000    | 0.10
Q54 | TC4*,TC311,TC312  | errorRate               | %            | low   | 1       | 10      | 0.20
Q54 | TC4*,TC311,TC312  | throughput              | bytes/second | high  | 1600000 | 4000000 | 0.50

* Compute the values for: **cost_score**, **Q53_score** and **Q54_score**
* Compute the apdex value for all other metrics according to the frustration and satisaction thresholds defined above and the method described in [QoE Indicators Description](QoE-Indicators-Description).

For example in case of **onda** target, after the computaton you will end up to a partial output like the following table:

Target Site | Apdex/Metric | weigth | Apdex/Metric Value | Satisfied Counts | Tolerating Counts | Total Counts
------------|--------------|--------|--------------------|------------------|-------------------|-------------
onda | apdex_avgProvisioningLatency | 0.10 | 0.716 | 87  | 98  | 190
onda | cost_score                   | 0.10 | 0.623 | -   | -   | 190
onda | Q53_score                    | 0.40 | 0.772 | -   | -   | 506
onda | Q54_score                    | 0.40 | 0.869 | -   | -   | 508

the Q5 value for onda will be the weigthed average of such schema leading to 0.79 => 79 % (B)
