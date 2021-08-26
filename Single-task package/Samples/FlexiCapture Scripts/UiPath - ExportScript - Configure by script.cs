/**
 * This example shows how create an UiPath connector and its settings.
 * The connector is intended to export a document from the FlexiCapture work process.
 */

// Importing required namespaces.
using Abbyy.Connectors.Dms.ComFacade;
using Abbyy.Infrastructure.Logging;
using System.Collections.Generic;

// Create the Factory instance that produces connectors for different document management systems (DMS)
// and robotic process automation (RPA).
Factory factory = new Factory();

// Set callback for logging: log messages from Connector will appear in FlexiCapture Processing Task notes.
factory.Init(Processing);

// Create the connector settings by manual.
// There are several ways to get a connector settings.
// This example shows how create and initialize the settings by manual.
// To get the settings from an xml file see the 'UiPath - ExportScript - Configure by XML.cs' example.
IConnectorSettings connectorSettings = new ConnectorSettings();

// Set connection settings to Orchestrator.
// Both orchestrator address and tenant must be specified.
connectorSettings.DmsConnectionSettings.Server = "https://cloud.uipath.com";
connectorSettings.DmsConnectionSettings.Repository = "Tenant name";

// Set Orchestrator credentials.
// For Enterprise Orchestrator specify user name (or email) and password.
// When using Enterprise Orchestrator, you have to set authentication method below in DmsSpecificOptions to 'Basic'

// When using UiPath Cloud Platform Orchestrator:
// First set username equal to your Cliend Id and password equal to your User Key,
// Then specify Account Logical Name below in DmsSpecificOptions
// You can get Tenant Name, Account Logical Name, Client Id and User Key from API Access Information window, as described at:
// https://docs.uipath.com/orchestrator/reference/consuming-cloud-api#getting-the-api-access-information-from-the-automation-clouds-ui
connectorSettings.DmsConnectionSettings.Credentials.Username = "username/client_id";
connectorSettings.DmsConnectionSettings.Credentials.Password = "password/user_key";

// Set output file name.
// '<DocumentDefinition>' and '<Identifier>' are dynamically replaced tags.
// It is allowed to specify simple static name, for instance, 'My_file_name'.

connectorSettings.DmsDocumentDestination.FileName = "<DocumentDefinition>_<Identifier>";

// Set UiPath Orchestrator queue name.
connectorSettings.MappedFields.DmsMetadataTemplate.Name = "QueueName";

// Set fields mapping.
Dictionary<string, string> pairs = new Dictionary<string, string>()
{
    // The first value is a full path to the field in FlexiCapture, the second - field key in the UiPath Orchestrator transaction.
    { @"Invoice Layout\InvoiceNumber", "InvoiceNumber" },
    { @"Invoice Layout\InvoiceDate", "InvoiceDate" },
    { @"Invoice Layout\Total", "Total" },
    { @"Invoice Layout\LineItems\OrderNumber", @"LineItems\OrderNumber" },
    { @"Invoice Layout\LineItems\ArticleNumber", @"LineItems\ArticleNumber" },
    { @"Invoice Layout\LineItems\Description", @"LineItems\Description" },
    { @"Invoice Layout\LineItems\Quantity", @"LineItems\Quantity" },
    { @"Invoice Layout\LineItems\UnitPrice", @"LineItems\UnitPrice" },
    { @"Invoice Layout\LineItems\Currency", @"LineItems\Currency" },
    { @"Invoice Layout\LineItems\TotalPriceNetto", @"LineItems\TotalPriceNetto" },
};

connectorSettings.MappedFields.FieldPairs = new MappedFields(pairs).FieldPairs;

// Set saving options for an export image.
IExportImageSavingOptions exportImageSavingOptions = FCTools.NewImageSavingOptions();
exportImageSavingOptions.AddProperFileExt = true;
exportImageSavingOptions.Format = "pdf";

// There are several other options that can be specified.
//exportImageSavingOptions.AddProperFileExt = true;
//exportImageSavingOptions.ColorType = "FullColor"; // "GrayScale" or "BlackAndWhite"
//exportImageSavingOptions.FieldsToRedact.Add(Document.Field(@"Invoice Layout\InvoiceNumber"));
//exportImageSavingOptions.Format = "pdf-a";
//exportImageSavingOptions.ImageCompressionType = TImageCompressionType.ICT_Lzw;
//exportImageSavingOptions.PdfAVersion = TPdfAVersion.PAV_3u;
//exportImageSavingOptions.PdfTextSearchArea = TPdfTextSearchAreaType.PTSAT_AllPages;
//exportImageSavingOptions.Quality = 90;
//exportImageSavingOptions.RecognitionLanguages = "German";
//exportImageSavingOptions.Resolution = 96;
//exportImageSavingOptions.SaveAttachmentsToPdf = true;
//exportImageSavingOptions.ShouldOverwrite = true;
//exportImageSavingOptions.UseMRC = false;

connectorSettings.DmsSpecificOptions = new Option[] {
// Authentication type(Optional for Enterprise Orchestrator, required for UiPath Cloud Platform Orchestrator)
// When using Enterprise Orchestrator either don't specify the value or set it to 'Basic'
// When using UiPath Cloud Platform Orchestrator set value to 'OAuth2'
    new Option(new KeyValuePair<string, object>("AuthenticationType", "OAuth2")),
        
// When using UiPath Cloud Platform Orchestrator provide account logical name
// You can get value of account logical name from API Access Information window, as described at:
// https://docs.uipath.com/orchestrator/reference/consuming-cloud-api#getting-the-api-access-information-from-the-automation-clouds-ui

// When using Enterprise Orchestrator, this option is ignored and shouldn't be specified.
    new Option(new KeyValuePair<string, object>("AccountLogicalName", "ACCOUNT_LOGICAL_NAME")),

// Provide reference value for UiPath queue item.
//new Option(new KeyValuePair<string, object>("Reference", Document.Batch.Id.ToString())),

// Provide name of UiPath Organization Unit when using UiPath Organization Units / Folders.
//new Option(new KeyValuePair<string, object>("OrganizationUnitName", "ORG_UNIT_NAME")),
};

// Option to enable/disable saving serialized files to result queue item.
// To enable saving serialized files to result queue item set it to 'true'.
// To disable it set value to 'false'.
connectorSettings.SaveDocumentImages = true;

// Create an UiPath connector instance.
using (IAfcDmsConnector connector = factory.CreateConnector(DmsConnectorType.UiPath))
{
    // Initialize the connector settings.
    connector.Init(connectorSettings);

    // Export the document to UiPath Orchestrator queue.
    connector.Export(Document, exportImageSavingOptions);
}
