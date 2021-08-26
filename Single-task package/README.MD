# Single-Task Package
## Architecture
The ABBYY FlexiCapture 12 single-task integration solution for UiPath comprises:

* Abbyy.Activities.UiPath.FlexiCaptureActivities NuGet package containing the Capture Data activity. Capture Data activity is used for sending files to ABBYY FlexiCapture 12 and getting the processing results in synchronous mode. Note:The activity only works with files containing one document each (i.e. each document must be placed in a separate file).
* FlexiCapture Export Connector for UiPath. It creates a transaction in the queue of the Orchestrator at the export stage, in which it stores the image file and metadata.