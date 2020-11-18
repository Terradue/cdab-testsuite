The command line tools rely on a configuration file in YAML format in which information about the various providers is configured. The YAML format is hierarchical and this page will describe each of the nodes for every level.

The software suite contains a [sample configuration file](../blob/master/src/cdab-client/config.sample.yaml) that is ready to use apart from the user credentials which have to be replaced with correct ones.

The following sections explain he configuration which consists of two main nodes:

* [**global**](#global-node): Contains settings that apply globally, over different service providers and test scenarios.
* [**service_providers**](#service_providers-node): Contains a node for each configured Target Site of the benchmark.
* [**data**](#data-node): Contains the [Catalogue Baseline Sets](Catalogue-Sets#baseline-sets) definition.

## `global` node

The following settings have a general scope and apply across providers.

* `country_shapefile_path`: Path to the shapefile containing country borders; used for building complex queries using country coverage in [TC202](Test-Scenarios-Description#tc202-complex-query-geo-time-filter) and [TC212](Test-Scenarios-Description#tc212-complex-query-from-cloud-services).

The following settings are related to processing-related test scenarios and apply across providers.

* `docker_config`: The location of the Docker authentication file (*config.json*). This file is required to authenticate with the CDAB Docker repository to install the image for the testing on the virtual machine. This type of file can be obtained by running the following command and authenticating with the repository username and password.
* `connect_retries`: Number of attempts to connect to virtual machines via SSH.
* `connect_interval`: Interval between attempts to connect to virtual machines via SSH (in seconds, fraction also possible).
* `ca_certificate`: An array of optional certificates to be installed on virtual machines used for processing.
* `max_retention_hours`: The number of hours after which previously created virtual machines are considered idle (in case they were not deleted properly during test execution, check takes place before creating a new VM on a service provider).

## `service_providers` node

Under this node, each key correspond to a configured Target Site in the benchmark. Each section has a key node that should be identified after the Target Site. Under that key node there are several node for settings related to that provider.

* `max_catalogue_thread`: Maximum number of thread querying the target catalogue service in parallel.
* `max_download_thread`: Maximum number of thread downloading the target download service in parallel.

* [**`data`**](#service_providersnamedata-node): A node with further settings related to catalogue search and download.
* [**`compute`**](#service_providersnamecompute-node): A node with further settings related to remote execution of test scenarios.
* [**`storage`**](#service_providersnamedata-node): A node with further settings related to storage of user-produced data.

The following sections explain the more complex nodes more in detail.

### `service_providers.<name>.data` node

These settings regard the data access of each provider. They must be comprehensive and thus may be complex, especially the definition of collections. It is recommeded to use the sample configuration file and adjust only the credentials. These settings are relevant for all [Test Scenarios](Test-Scenarios-Description).

* `url`: Data Access Entry Point, usually the URL to the catalogue.
* `credentials`: Data Access Credentials, usually in the form `username:password` or `apiKey`.
* `s3_key_id`: Data Access Object Storage key id used for download using S3 protocol.
* `s3_secret_key`: Data Access Object Storage secret key used for download using S3 protocol.

* [**`catalogue`**](#service_providersnamedatacatalogue-node): Data Access Catalogue configuration of the Target Site. This section defines all the catalog and collections available at the Target Site and used for the benchmark.

#### `service_providers.<name>.data.catalogue` node

This section has a unique node **`sets`** that declares all the [Catalogue Sets](#catalogue-or-data-set) of the Target Sites. in this section, each **set** is identified by an arbitrary key and the is additionally classified according to the **set** type:

* `type`: defines the **set** type.
  * `baseline`: The catalogue collections' **set** refers to [Common Baseline Sets](Catalogue-Sets#common-baseline-sets) defined globally in the [`data` node](#data-node). It requires the `reference_set_id` setting to set the Baseline set identifier. Those sets are used in [TC5O1](Test-Scenarios-Description#tc501), [TC5O2](Test-Scenarios-Description#tc502), [TC6O1](Test-Scenarios-Description#tc601) & [TC6O2](Test-Scenarios-Description#tc602)).
  * `local`: The catalogue collections' **set** is locally managed by the Target Site and defines the *Online* data sets. Those sets are used is [TC503](Test-Scenarios-Description#tc503) to measure the Data Offer Consistency.
* `reference_set_id`: (Mandatory for type `baseline`) The reference set id link the current **set** with a globally defined [Common Baseline Sets](Catalogue-Sets#common-baseline-sets).
* `reference_target_site`: The reference Target Site for the [Comparative Catalog] function of the Test Case. This is the key used to defined the Target Site in the [`service_providers` node](#service_providers-node).

### `service_providers.<name>.compute` node

These settings configure the processing within the cloud infrastructure of the service providers. They are relevant for [Remote Test Scenarios](Test-Scenarios-Description#remote-test-scenarios).

The **cdab-remote-client** uses OpenStack which all DIAS providers support. Most of the values for the various keys can be obtained from the OpenStack dashboard of the cloud environment in question (by inspecting the file *clouds.yaml* that can be downloaded under *API Access > Download OpenStack RC File > OpenStack clouds.yaml File*), others have to be set with knowledge of concrete items that are configured on the cloud environment in question.

* `connector`: The value has to be *openstack* to which it defaults (making it optional in this case).
* `auth_url`: Authentication access point. Obtain value from `auth_url` key in *clouds.yaml*.
* `username`: Cloud username (same username as for access to the OpenStack dashboard).
* `password`: Cloud password (password for user).
* `project_id`: Project ID. Obtain value from `project_id` key in *clouds.yaml*. This setting is optional.
* `project_name`: Project name. Obtain value from `project_name` key in *clouds.yaml*.
* `user_domain_name`: User domain name. Obtain value from `user_domain_name` key in *clouds.yaml*.
* `region_name`: Authentication region name. Obtain value from `region_name` key in *clouds.yaml*. This setting is optional.
* `interface`: Interface. Obtain value from `interface` key in *clouds.yaml*.
* `identity_api_version`: Identity API version, Obtain value from `identity_api_version` key in *clouds.yaml*.
* `volume_api_version`: Volume API version (set value to *2* if version *3* is not supported).
* `key_name`: Name of predefined public key for SSH connection to new virtual machine. On the OpenStack dashboard, key pairs can be created on the OpenStack dashboard and the private key can be downloaded, check under *Compute > Key Pairs*.
* `image_name`: Name of image to be used for new virtual machine. On the OpenStack dashboard, choose from *Compute > Images*.
* `flavor_name`: Name of flavour (hardware characteristics) for new virtual machine. On the OpenStack dashboard, check under *Compute > Instances > Launch Instance > Flavours/Flavors*. The value can also be an array, e.g. *['flavourA', 'flavourB']*
* `network_name`: Name of network to which new virtual machine is connected. On the OpenStack dashboard, check under *Network > Networks*. This setting is optional. The value can also be an array, e.g. *['networkA', 'networkB']*
* `security_group`: Name of security group for new virtual machine (optional; this setting might be necessary in order to permit remote access to virtual machines). On the OpenStack dashboard, check under *Network > Security groups*, if available. This setting is optional.
* `floating_ip`: Explicitly assign floating IP (set this to *True* if public IP addresses are not assigned automatically at the creation of a virtual machine and otherwise to *False*). On the OpenStack dashboard, check under *Network > Floating IPs*, if available.
* `floating_ip_network`: Network from which to assign floating IP. This setting is optional.
* `private_key_file`: Location of the private key file for SSH connections to virtual machine (must correspond to public key in `key_name`).
* `remote_user`: User on virtual machine for SSH connections.
* `use_volume`: Create an external volume for docker image and test execution; this is useful for flavours that have very limited main disk. The size of the additional disk is 100 GB.
* `private_key_file`: Location of the private key file for SSH connections to virtual machine (must correspond to public key in `key_name`).
* `remote_user`: User on virtual machine for SSH connections.
* `vm_name`: Preferred name of virtual machines to be created (sequential number is appended).
* `cost_monthly`: Monthly cost for VM of specified flavour (instance type or machine type, see sections below). The default is *0*. If there is more than one flavour, the value has to be an array of the same size.
* `cost_hourly`: Hourly cost of VM of specified flavour. The default is *0*. If there is more than one flavour, the value has to be an array of the same size.
* `currency`: Payment currency. The default is *EUR*.


### `service_providers.<name>.storage` node

These settings configure storage access for the service providers. They are relevant for TS07.

For OpenStack-compatible storage offers, the following settings can be used (same as in the [`service_providers.<name>.storage`](#service_providersnamecompute-node) section):

* `auth_url`: Authentication access point.
* `username`: Cloud username.
* `password`: Cloud password (password for user).
* `project_id`: Project ID.
* `project_name`: Project name.
* `user_domain_name`: User domain name.

For storage offers supporting the S3 protocol, the following settings have to be used instead:

* `s3_service_url`: The service URL for S3 access.
* `s3_key_id`:  The S3 key ID, a combination of numbers and letters, usually 20 characters long.
* `s3_secret_key`: The corresponding S3 secret key, another string consisting of numbers, letters and slashes, usually 40 characters long.


In all cases, settings for the size range of the test content can be made:

* `min_upload_size`: Minimum size of generated file to upload (and subsequently download), in MB.
* `max_upload_size`: Maximum size of generated file to upload (and subsequently download), in MB.


## `data` node

This global section defines the It has a unique node [**`sets`**](#catalogue-or-data-set) that declares all the [Common Baseline Sets](Catalogue-Sets#common-baseline-sets) of the benchmark. in this section, each **set** is identified by an arbitrary key used then globally among all [`service_providers.<name>.data.catalogue` node](#service_providersnamedatacatalogue-node) as a reference.

## Common Configuration Nodes

This section describes common configuration node.

### Catalogue or Data **set**

This configuration section define a **set** of data collections. It is used globally in the [`data` node](#data-node) to declare all the [Common Baseline Sets](Catalogue-Sets#common-baseline-sets) of the benchmark and in the [`service_providers.<name>.data.catalogue` node](#serviabout
> 💡 More information about the sets and their relationships in the [Catalogue Sets](Catalogue-Sets) page.

Each value of the `sets` dictionary contains:

* `name`: Name and label of the **set**
* **`collections`**: this dictionary section declares all the [Data collection](#data-collection) of the **set**. Each collection is identified by an arbitrary key.
* **`parameters`**: this array section declares all the [OpenSearch Parameters](#opensearch-parameter) common to all **collections**.

#### Data **collection**

Each value of the `collections` dictionary contains:

* `label`: Label of the **collection** used in the benchmark reporting to label the collections metrics
* **`parameters`**: this array section declares all the [OpenSearch Parameters](#opensearch-parameter) to build the catalogue query that defines the collection

#### OpenSearch **parameter**

Each **collection** in a **set** is defined by the filters used to build the catalogue query towards the Target Site. Since all target Sites do not used the same filters, the [OpenSearch](https://github.com/dewitt/opensearch/blob/master/opensearch-1-1-draft-6.md) and its extensions are used as a common standard to define those filters. The Test Suiste then maps the filters accordingly for each Target Site's data access catalogue search function.

Each item of the `parameters` array contains:

* `key`: unique key in the array identifying the filter
* `full_name`: OpenSearch Fully Qualified Name of the filter in the form `{namespace}name`. The following namespaces and their filters are supported:
  * `http://a9.com/-/spec/opensearch/1.1/`: Common [OpenSearch](https://github.com/dewitt/opensearch/blob/master/opensearch-1-1-draft-6.md) filters
  * `http://a9.com/-/opensearch/extensions/geo/1.0/`: Geographic filters of the [OpenSearch Geo and Time Extensions](https://www.ogc.org/standards/opensearchgeo)
  * `http://a9.com/-/opensearch/extensions/time/1.0/`: Time filters of the [OpenSearch Geo and Time Extensions](https://www.ogc.org/standards/opensearchgeo)
  * `http://a9.com/-/opensearch/extensions/eo/1.0/`: Earth Observation filters of the [OpenSearch Extension for Earth Observation](https://www.ogc.org/standards/opensearch-eo)
* `value`: value of the filter
* `label`: label of the filter used in the benchmark reporting to label the metrics.
