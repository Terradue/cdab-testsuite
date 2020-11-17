The benchmark performed in the previous Test Scenarios reports raw performance metrics from the different services offered by the Target Sites: Catalogue & Data Access, Cloud Computing and Storage. It is therefore not specific to any application or user type. Nevertheless, experienced users who elaborate value-added information from Copernicus data would be more interested about evaluations targeted to their specific application. In order to assess these usage-specific performances, the Test Suite implements procedure for benchmarking [five end-to-end use cases](../tree/develop/Use%20Cases).

Those Use Cases are part of a larger End-to-End Scenario that must includes manual review procedures not covered by this Test Suite.

## End-to-End User Scenario

The End-To-End scenario is guided by a template scenario represented by the directed graph hereafter that describes all the major steps executed during the End-to-End scenario for a typical use case.

[image]: images/UseCasesDiag.png "Use Case Diagram" 
![Alt text][image]

Those End-to-End aims at benchmarking Target Sites with a comprehensive service offering including cloud computing and storage. They therefore do not apply to Data Access Hubs.

In the guide we will perform the automatic tests (in blue) and the "application integration" of Step #2 since thay use Test Scenarios and generic procedures implemented by the Test Suite.

We will perform then for the [Use Case Scenario #1](../tree/develop/Use%20Cases/Scenario%201%20-%20NDVI%20Mapping) applicable to all End-To-End Use Cases.

## Prepare the Test Suite Configuration

We will tailor the Test Suite configuration file `config.yaml` in order to meet the data requirements of the Use Case. Indeed, the [Catalogue Sets](Catalogue-Sets) must be configured to define only the data collections targeted by the Use Case and defined in the Use Case [variables](../tree/develop/Use%20Cases/Scenario%201%20-%20NDVI%20Mapping#question--context) `Use Case Data Collection`. In our use case, this is `Sentinel-2 L1C` that is defined to process NDVI.

For the purpose of this guide, a Use Case sample config with the data collection has been prepared. Download the [`config.yaml`](../blob/develop/Use%20Cases/Scenario%201%20-%20NDVI%20Mapping/config.sample.yaml) to your working directory. It only contains collection defintion for the `Sentinel-2 L1C` datasets.

Edit the `config.yaml` to set properly the credentials for the Target Sites you want to benchmark. All configuration details are available in the [Configure the Test Suite](Configure-The-Test-Suite.md) page.

## Start the Test Site Container

We will start the Test Site docker container we will use for this benchmark session.

```console
docker run --detach --name testsite-2 esacdab/testsuite:latest
```

## Data Offer Richness Test Procedure

The data offer richness test procedure aims at measuring the Data Offer adequacy of the target site with regards to the Use Case.

To retrieve the metrics necessary to compute the adequacy, we will run the Test Scenarios [TS05](Test-Scenarios-Description#ts05) and [TS06](Test-Scenarios-Description#ts06).

Execute Test Scenario 05 (replace `Target` with the Target Site key)

```console
docker exec -it testsite-2 cdab-client -v -tsn=testsite-2 -tn=Target TS05
```

Once completed, copy the results

```console
docker cp testsite-2:TS05Results.json testsite-2-TS05-Target-results.json
```

Execute Test Scenario 06 (replace `Target` with the Target Site key)

```console
docker exec -it testsite-2 cdab-client -v -tsn=testsite-2 -tn=Target TS06
```

Once completed, copy the results

```console
docker cp testsite-2:TS06Results.json testsite-2-TS06-Target-results.json
```

