# Multi-Task Package
## Architecture
The ABBYY FlexiCapture 12 multi-task integration solution for UiPath comprises the following
components:

* UiPath Activity Package.
* UiPath Processes.
* UiPath Queues, which is required in order for the default processes to function properly. The ABBYY FlexiCapture 12 multi-task integration solution for UiPath requires the addition of the following queues: Monitor Queue, Verifier Queue, and Fetcher Queue. Queues are used for storing the IDs of the batches with which the processes are currently working.
* UiPath Assets/Credential, which makes setting up a ABBYY FlexiCapture server connection easier. These values are used by all of the processes. The ABBYY FlexiCapture 12 multi-task integration solution for UiPath requires the addition of the FlexiCapture Credentials credential and the following assets: FlexiCapture Server Address, FlexiCapture Company, FlexiCapture Project Name.

