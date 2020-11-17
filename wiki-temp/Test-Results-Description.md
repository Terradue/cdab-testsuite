This page give a brief overview of how are structured the results in the `json` files produced by the [Command Line Tools](Command-Line-Tools.md)

For this description, we will use a the result produced by the [Test Scenario TS01](Test-Scenarios-Description#ts01) on [Copernicus Open Access Hub](https://scihub.copernicus.eu/) in the [Run your first Test Scenario](Run-your-first-Test-Scenario.md) guide.

## Test Scenario Result File Name

The result file is always named by the Test Scenario ran (TSXXResults.json). In this case, `TS01Results.json`

## General Benchamrk Information

The first key/value items of the file content informs about general information of the Test Scenario Execution

```json
{
    "jobName": null,
    "buildNumber": null,
    "testScenario": "TS01",
    "testSite": "testsite-1",
    "testTargetUrl": "https://scihub.copernicus.eu/apihub",
    "testTarget": "ApiHub",
    "zoneOffset": "+00",
    "hostName": "0a7c0ab615fe",
    "hostAddress": "172.17.0.3",
...
```

* `jobName`: reports the environment variable `JOB_NAME` when set (used by the automatic test systems)
* `buildNumber`: reports the environment variable  `BUILD_NUMBER` when set (used by the automatic test systems)
* `testScenario`: Identifier of the [Test Scenario](Test-Scenarios-Description#test-scenarios) executed
* `testSite`: Identifier of the test site specified in the command line arguments
* `testTargetUrl`: Target Site endpoint base url of the service used for this Test Scenario
* `testTarget`: Identifier of the Target Site
* `hostName`: Hostname of the Test Site machine. In this case, the docker container hostname
* `hostAddress`: IP address of the Test Site machine. In this case, the docker container IP address

## Test Cases Results

The section `testCaseResults` is an array of items. Each item report the metrics of each Test Case

### Test Case Information

The first key/value items of each test case content informs about general information of the Test Scenario Execution

```json
...
    "testCaseResults": [
        {
            "testName": "TC101",
            "className": "cdabtesttools.TestCases.TestCase101",
            "startedAt": "2020-11-11T13:36:49.2875490+00:00",
            "endedAt": "2020-11-11T13:36:50.1831040+00:00",
            "duration": 895,
...
```

* `testName`:  Identifier of the [Test Case](Test-Scenarios-Description#test-case) executed
* `className`: Identifier of the class implementing the Test Case
* `startedAt`: Timestamp of the beginning of the Test Case (ISO8601)
* `endedAt`: Timestamp of the completion of the Test Case (ISO8601)
* `duration`: Duration of the Test Case (seconds)

### Test Case Metrics

The benchmark results are in this `metrics` section organized as an array of **metric** results

```json
...
            "metrics": [
                {
                    "name": "avgResponseTime",
                    "value": 848,
                    "uom": "ms"
                },
                {
                    "name": "peakResponseTime",
                    "value": 848,
                    "uom": "ms"
                },
                {
                    "name": "errorRate",
                    "value": 0.0,
                    "uom": "%"
                },
                {
                    "name": "avgConcurrency",
                    "value": 1.98,
                    "uom": "#"
                },
                {
                    "name": "peakConcurrency",
                    "value": 2,
                    "uom": "#"
                }
            ]
...
```

* `name`: Identifier of the metric
* `value`: Measured value of the metric
* `uom`: unit of measure of the metric

### Additional Test Case Metadata

Some Test Case reports additional runtime information

* `searchFiltersDefinition`: generated random filters used by the catalogue search (TC2XX)
