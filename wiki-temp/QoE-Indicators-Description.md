# The Copernicus Sentinels Data Access Benchmark Service

The objective of the Copernicus Sentinels Data Access Worldwide Benchmark is to provide impartial and reliable information on the Sentinels data accessibility from the access points under ESA management, building a comprehensive and objective overview of the conditions actually experienced by users.

In order to achieve its objectives, the benchmarking service encompasses a combination of measurements, interpretation and communication activities that include:
* The systematic execution of benchmarking operations, simulating typical user scenarios from different test sites located in Europe and around the world;
* The cleaning, analysing and interpretation of the results;
* The presentation of the results towards various actors;
* The monitoring of the results and of the benchmark quality over time; and
* The implementation of service evolutions as appropriate.

For the sake of the benchmarking service, "typical" operations performed by the Sentinels users’ can be sketched according to [Test Scenarios Description](Test-Scenarios-Description)

The service operates an independent benchmarking of ESA’s hubs and DIASes from a worldwide network of 25+ Test Sites.

The Test Sites are managed remotely as part of a broad architecture designed to provide a regular and automated monitoring of the target sites in an automated way. A complex orchestration of operations from the various test sites to the various Target Sites is operated, whereby each test site issues one or more requests towards one or more target sites, according to a pre-defined test-to-target matrix. The results are systematically stored, analysed and reported.

The main pillars of the benchmarking service architecture are:

* a network of Test Sites, that perform the benchmarking operations towards the pre-defined target sites;
* the Orchestration function, in charge of managing the benchmarking operations;
* the Analysis and Reporting function;
* the Calibration and Validation function, ensuring a continuous monitoring of the service reliability;
* a Public Software Repository service (this repository); and
* the Service Management, in charge of the overall management including possible evolutions.

The service results are captured on different kind of deliverables to different stakeholders with different distribution policies such as:
* The Service Design Document, presenting the main characteristics of the service and providing key information that is needed to interpret the service results. 
* The Service Specific Reports, presenting the service results for a given target site (e.g. the ESA’s hubs or the DIAS); and
* The Service Summary Report, presenting an overview of the core benchmarking results to the Copernicus governance.

## Quality-Of-Experience indicators

The benchmarking service must assess the quality of the data access services as experienced by the users. An evolution is thus required from the concept of Quality of Service (QoS) (that is measured at the data access points from the service providers) to the wider idea of Quality of Experience (QoE), where the key factor is not only to measure the data access performance parameters, but the perception that users have of the provided service. 

The key questions to be addressed for evaluating the user’s perception can be summarised as follows:
* **Q1 Copernicus Collections Richness** - How rich is the Copernicus Sentinels data offer?
* **Q2 Reactivity** - How reactive is the target site in responding to the user’s request?
* **Q3 Discovery** - How efficently can users find (and visualize) the desired Sentinels products?
* **Q4 Download** - How quickly can users download the identified Sentinels product/s of interest?
* **Q5 Cloud Computing** - How efficient and cost-effective is the Sentinels data access and processing in the cloud?

In order to answer these questions in a simple, yet representative, objective and quantifiable way, the above QoE indicators were defined that effectively synthesize the measured performances. Each target site can then be evaluated according to its set of QoEs and the derived classification.
The above indicators are automatically computed: for each functionality, dedicated requests are automatically executed tens or even thousands of times during the reporting period and from different test sites in Europe and around the world. 

*How can all these results be synthesized in an effective way?*

A hierarchical computation was developed that progressively refines and combines the obtained results, based on the following principles:
* For most of the metrics MX, we derive a statistic representation of the percentage of users that are satisfied, tolerating or dissatisfied with the service. We do so though a so-called application performance index (or APDEX) that is closer to the user perception with respect to the engineering result.
* The APDEX, which was defined from [Apdex Users Group](https://www.apdex.org/), leverages on the distinction of three performance zones (i.e. Satisfied, Tolerating and Frustrated) and weighs the number of times the user experience falls in one of these zones (with weights of 1, 0.5 and 0 respectively) over the totality of the relevant test counts. In an automatic performance evaluation system, like ours, the Tolerating Counts are the values of the Metric that are considered acceptable, whereas the Satisfied Counts are the values of the Metric that are considered more than acceptable.
![alt text](../images/apdexUnderstanding.png?raw=true "Apdex")
* By definition, the APDEX values fall between 0 and 1 and are proportional to the user satisfaction levels (i.e. APDEX=0 means that no user is satisfied while APDEX=1 indicates that all user samples were in the “satisfied” zone)
* Some metric values are already returned as a percentage (for example for Q1 computation), so in this case no APDEX is computed, but the current value (also ranging from 0 to 1 as the apdex score) is considered.
* In order to have a stable reference for the quality assessment of the target sites from the European perspective, the APDEXES relevant to the QoEs Q2, Q3, Q4, Q5 are computed only from a limited set of test sites located in Europe that constitute Europe’s Reference Test Sites and over the reporting period.

As stated above, the **QoE indicators** computed by the **benchmarking service** are based on a cloud infrastructure (**Test Sites**) running a **SW Test Suite** toward a given **Target site** (ESA’s hubs or the DIAS) launched from the Orchestrator with well defined configuration in order to gather a bulk [**Test Results**](Test-Results-Description) from different [**Test Scenarios**](Test-Scenarios) allowing statistical analysis on the data.

Also if the results of a few [Test Scenario runs](Run-your-first-Test-Scenario), could be not statistically meaningful for the computation of a QoE indicator as the ones provided from the benchmarking service, the workflow is the same and summarized as follows:

1. Results from the benchmarking operations are recorded in a number of elementary metrics that are computed from each test site
1. The elementary metrics are filtered to obtain corresponding application indexes (or APDEXs)
1. The APDEXs relevant to a given functionality (i.e. related to the questions Q1 to Q5) are weighted and combined as appropriate to obtain the corresponding QoE indicator 
1. The target site is classified (A,B,C,D or E) according to the QoE indicator values for each of the specific questions.

The following figure illustrates the above concept:

![alt text](../images/qoeClassification.png?raw=true "QoE Classification")

Further details on specific QoE indicators and computation sample are described [**here**](Compute-QoE-Indicators).
