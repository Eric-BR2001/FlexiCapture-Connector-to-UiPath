using Abbyy.Connectors.Dms.ComFacade;
using Abbyy.Infrastructure.Logging;
using System.Collections.Generic;

// Create the Factory instance that produces connectors for different document management systems (DMS)
// and robotic process automation (RPA).
Factory factory = new Factory();

/* Get Connector settings from the XML file

 There are several ways to address the file.
 1) The recommended way is to call
 		IConnectorSettings connectorSettings = factory.CreateConnectorSettings(Document.DocumentDefinition);
 The Connector will look for file with name "<Project Name (FCTools.ScriptContext.Project.Name)>_<Document Definition Name (Document.DocumentDefinition.Name)>.xml" (e.g. InvoicesProject_Invoice.xml)
 in the folder specified via registry [HKEY_LOCAL_MACHINE\Software\ABBYY\FlexiCapture\12.0\Connectors\UiPath] “XmlFolder” key.
 If file with required name is absent, then default.config.xml will be used. Default.config.xml should be also located in the folder specified via registry.

 2) You can specify the name of the file explicitly, e.g.: 
 		IConnectorSettings connectorSettings = factory.CreateConnectorSettings("Invoice.xml");
 The file will be looked for in the folder specified via registry [HKEY_LOCAL_MACHINE\Software\ABBYY\FlexiCapture\12.0\Connectors\UiPath] “XmlFolder” key.
 If file with required name is absent, then default.config.xml will be used. Default.config.xml should be also located in the folder specified via registry.

 3) You can specify the location of the file explicitly, e.g.: 
 		IConnectorSettings connectorSettings = factory.CreateConnectorSettings("C:\\XmlFolder\\Invoice.xml");
*/
// Set callback for logging: log messages from Connector will appear in FlexiCapture Processing Task notes
factory.Init(Processing);

IConnectorSettings connectorSettings = factory.CreateConnectorSettings(Document.DocumentDefinition, DmsConnectorType.UiPath);

// Create Connector object
using (IAfcDmsConnector exportConnector = factory.CreateConnector(DmsConnectorType.UiPath))
{
    // Initialize Connector object with settings
    exportConnector.Init(connectorSettings);

    // Export document to DMS according to settings
    exportConnector.Export(Document);
}