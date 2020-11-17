# Getting started ⚡️

* ▶️ [Test Site Preparation (this page)](#test-site-preparation)
* ⏬ [Run your first Test Scenario](Run-your-first-Test-Scenario)
* ⏬ [Perform an End to End Use Case](Perform-an-E2E-Use-Case)
  

## Test Site Preparation

The Test Suite and the Test Scenarios are executed from a **Test Site** which can be any computer anywhere.

### Software requirements

For running this Test Suite properly, you only need docker installed. The getting started guide on Docker has detailed instructions for setting up Docker on [Mac](https://docs.docker.com/docker-for-mac/install), [Linux](https://docs.docker.com/install/linux/docker-ce/ubuntu) and [Windows](https://docs.docker.com/docker-for-windows/install).

Once you are done installing Docker, test your Docker installation by running the following:

```console
$ docker run hello-world

Hello from Docker.
This message shows that your installation appears to be working correctly.
...
```

### Using a Test Suite container

It is higly recommended to use the Test Suite from a container to ensure benchmark isolation as we do in this benchmark.
The Test Suite is packaged and delivered as a [Docker](https://docker.com) image ready to run Test Scenarios.

You can either

👷‍♀️ Build and deploy your own Test Suite

or

🚢 Use the [latest release version](https://hub.docker.com/repository/docker/esacdab/testsuite) available on Docker Hub as [esacdab/testsuite:latest](https://hub.docker.com/r/esacdab/testsuite)

Pull the latest version of the image with this command

```console
$ docker pull esacdab/testsuite:latest

latest: Pulling from esacdab/testsuite
af4b0a2388c6: Already exists
c62fd00f9e96: Pulling fs layer
6689ef7e656b: Pulling fs layer
...
Digest: sha256:5b0f66149f946884af5a9993abe17ff7c2717070c6b2fd9b95091301e3016de4
Status: Downloaded newer image for esacdab/testsuite:latest
docker.io/esacdab/testsuite:latest
```

In this guide, we assume using a locally packaged Test Suite so we will refer to `esacdab/testsuite:latest` as the docker image. Please substitute with your image tag.

### Prepare the Test Suite configuration (config.yaml)

In this Getting Started Guide, we will copy the [sample configuration file](../blob/master/src/cdab-client/config.sample.yaml) that is ready to be used.
The only necessary step is to set the credentials with yours.

> 💡 A detailed description of all the configuration elements are available in the [Configure the Test Suite](Configure-The-Test-Suite) page.

First download the [config.sample.yaml](../blob/master/src/cdab-client/config.sample.yaml) to your working directory as `config.yaml`

For instance, we will set the credentials for the SciHub and MUNDI DIAS.

Scihub uses the username and the password that you set at the registration of the [Copernicus Open Access Hub](https://scihub.copernicus.eu/).

Edit the downloaded `config.yaml` and find the `SciHub` section in `service_providers` node. Change the `username:password` string by your username and password.

```yaml
...
  SciHub:
    max_catalogue_thread: 1
    max_download_thread: 2
    data:
      url: https://scihub.copernicus.eu/dhus
      credentials: username:password
...
```

Do the same for the `MUNDI` section. Here in addition, you must set your S3 key ID and the corresponding secret key as described in the section "Download with S3 API" in [Mundi Web Services documentation](https://mundiwebservices.com/help/documentation). Furthermore you must edit the cloud access and account settings for the processing-related scenarios.

```yaml
...
   MUNDI:
    max_catalogue_thread: 5
    max_download_thread: 2
    data:
      url: https://mundiwebservices.com/acdc/catalog/proxy/search
      credentials: username:password
      s3_key_id: id
      s3_secret_key: "secret"
      ...
    compute:
      auth_url: https://iam.eu-de.otc.t-systems.com:443/v3
      username: "cloud-username"
      password: "cloud-password"
      project_name: "project-name"
      user_domain_name: "user-domain-name"
      ...
      
```

🎉  Your are now ready for the next step:

> ⏭️ [Run your first Test Scenario](Run-your-first-Test-Scenario)
