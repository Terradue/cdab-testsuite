The command line tools are the main executors of the [Test Scenarios](Test-Scenarios-Description). They are used according to the type of Test Scenario performed

* [**Local Test Scenarios**](Test-Scenarios-Description#Local-Test-Scenarios) are  locally executed measuring metrics of the various functions from the local Test Site towards the Target Site. They are run using [**cdab-client**](#cdab-client) CLI.
* [**Remote Test Scenarios**](Test-Scenarios-Description#Remote-Test-Scenarios) are remotely executed on a virtual machines within the service providers' cloud infrastructure (when available) measuring metrics of the various functions directy within the Target Site. They are run using [**cdab-remote-client**](#cdab-remote-client) CLI.

## cdab-client

```console
Usage: cdab-test [TEST SCENARIOS]
Launch test scenarios to a target site
If no test scenario is specified, the generic TS01 is executed.

Options:
      --conf=VALUE           YAML file containing the configuration.
      --tu, --target_url=VALUE
                             target endpoint URL (default https://scihub.
                               copernicus.eu/apihub). Overrides configuration
                               file.
      --tc, --target_credentials=VALUE
                             the target credentials string (e.g. username:
                               password). Overrides configuration file.
      --tn, --target_name=VALUE
                             the target identifier string. Mandatory to use the
                               target site configuration from file.
      --tsn, --testsite_name=VALUE
                             the test site identifier. Mandatory to use the
                               test site configuration from file.
      --lf, --load_factor=VALUE
                             Load Factor. Mainly used as a constant to
                               calculate the number of run to make per test cases
  -v                         increase debug message verbosity.
  -h, --help                 show this message and exit.
```

**cdab-client** performs the following steps:

* Check the command line arguments, configuration and selected test scenarios and starts the test making sure that there is no misconfiguration.
* Prepare each Test Cases in a separate thread of the selected Test Scenario and chain them accordingly
* Fire the Test Cases!
* When all threads have completed, aggregate the metrics of each Test Case above and produce a `TSXXResults.json` [result file](Test-Results-Description) containing the benchmark result of the executed Test Scenario and an updated `junit.xml`.

## cdab-remote-client

```console
cdab-remote-client version 1.28 (c) 2020 Terradue Srl.

USAGE: libexec/cdab-remote-client.py3 [OPTIONS] <test-scenario>

OPTIONS
    -h                        Display this help and exit
    -v                        Display more information during processing
    -ml                       Allow mixed log output (relevant for multiple parallel runs)
    -conf=<file>              YAML file containing the remote configuration
                              Default value: /opt/cdab-remote-client/etc/config.yaml
    -vm=<number>              Number of virtual machines to be run in parallel (min: 1)
                              Default value: 1
    -lf=<number>              Load factor (min: 1)
                              Default value: 1
    -sp=<name>                Service provider for test execution (as defined in configuration file)
    -ts=<name>                Target site for querying (as defined in configuration file)
    -te=<url>                 Endpoint URL for remote target calls (overrides settings from target site set with -ts)
    -tc=<username:password>   Credentials for target (overrides settings from target site set with -ts)
    -psw=<name>               CWL workflow file (TS15 only) replacing default file
    -psi=<name>               Text file with input product URLsfor workflow (TS15 only)
    -i=<name>                 Docker image identifier (URL)
                              Default value is automatically determined
    -a=<name>                 Docker authentication file (config.json)
                              Default value is automatically determined
    -n=<name>                 Test site name (parameter for cdab-client call)

ARGUMENTS
    <test-scenario>           Test scenario ID
                              Possible values: TS11, TS12, TS13, TS15.1, TS15.5
```

**cdab-remote-client** performs the following steps:

* Check the command line arguments, configuration and selected test scenarios and starts the test making sure that there is no misconfiguration.
* The cloud environment to be used is obtained from the value of the `-sp` option which determines the service provider section in the configuration file to be used (values are taken from its [`compute`](Configure-The-Test-Suite#service_providersnamecompute-node) subsection).
* The target site parameters to be used are obtained from the value of the `-te` and `tc` options. Alternatively the `-ts` option is used; it determines the service provider section in the configuration file to be used (values are taken from its [`data`](Configure-The-Test-Suite#service_providersnamedata-node) subsection).
* If configured via the `floating_ip` key in the main configuration file, get the list of available floating IP addresses and make sure they are sufficient to perform all tests in parallel.
* Delete old virtual machines no longer in use according to the `max_retention_hours` global setting.
* Start a new thread for each requested virtual machine (`-vm` option) and do the following in parallel for each:
 
  * Create the virtual machine (using the `openstack server create` command or an equivalent for other providers)
  * If configured via the `floating_ip` key in the main configuration file, assign a floating IP address to the virtual machine (using the `openstack server add floating ip` command or an equivalent for other providers if applicable).
  * If configured via the `use_volume` key, create and attach the volume to the virtual machine (using the `openstack volume create` and `openstack server add volume` commands or equivalents for other providers if applicable) and partition, format and mount the volume.
  * Install Docker and start the Docker service (in case the key `use_volume` was set to *True*, change the local docker repository location to the new volume.
  * Transfer the Docker authentication file in order to be able to authenticate with the Terradue Docker repository.
  * Install the testsuite image containing the **cdab-client** tool (or other images or software needed for the test execution on the virtual machine).
  * Run the test scenario based on the command-line arguments, configuration settings and mapping of remote test scenarios onto **cdab-client** scenarios or other testing executables.
  * After conclusion extract the result files (`TSXXResults.json` and `junit.xml`) from the Docker container and download it.
  * If configured via the `use_volume` key, detach the volume from the virtual machine and delete it (using the `openstack server remove volume` and `openstack volume delete` commands or equivalents for other providers if applicable).
  * Delete the virtual machine (using the `openstack server delete` command or an equivalent for other providers).

* When all threads have completed, calculate the metrics described above and produce a `TSXXResults.json` [result file](Test-Results-Description) containing the information about the executed test scenario and an updated `junit.xml`.

