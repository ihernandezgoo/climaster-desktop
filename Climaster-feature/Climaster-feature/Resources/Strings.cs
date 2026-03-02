namespace Climaster_feature.Resources
{
    public static class Strings
    {
        // Títulos principales
        public const string AppTitle = "CliMaster Widget Diseinatzailea";
        public const string Configuration = "?? Konfigurazioa";
        public const string LayoutElements = "?? Layout Elementuak";
        public const string AndroidPreview = "?? Android Aurrebista";
        public const string QRCode = "?? QR Kodea";
        public const string JsonPreview = "?? JSON Aurrebista";

        // Widget Konfigurazioa
        public const string WidgetId = "Widget ID:";
        public const string WidgetSize = "Widget Tamaina:";
        public const string BackgroundColor = "Atzeko Kolorea (HEX):";
        public const string CornerRadius = "Ertz Erradioa:";
        public const string CurrentSize = "Uneko tamaina:";

        // Tamaño información
        public const string SizeInfo = "?? Tamainari buruzko informazioa";
        public const string MaxElements = "Gehienezko elementuak:";
        public const string CurrentElements = "Uneko elementuak:";
        public const string RecommendedElements = "Gomendatutako elementuak:";

        // Elementos
        public const string AddElements = "? Elementuak Gehitu";
        public const string Temperature = "??? Tenperatura";
        public const string Condition = "?? Baldintza";
        public const string Humidity = "?? Hezetasuna";
        public const string Wind = "?? Haizea";
        public const string Divider = "? Zatitzailea";
        public const string DailyForecast = "?? Eguneko Iragarpena";
        public const string HourlyForecast = "? Orduko Iragarpena";

        // Nombres de elementos para display
        public const string TempDisplay = "??? Uneko Tenperatura";
        public const string ConditionDisplay = "?? Uneko Baldintza";
        public const string HumidityDisplay = "?? Hezetasuna";
        public const string WindDisplay = "?? Haizearen Abiadura";
        public const string DividerDisplay = "? Zatitzaile Horizontala";
        public const string DailyDisplay = "?? Eguneko Iragarpena";
        public const string HourlyDisplay = "? Orduko Iragarpena";

        // Propiedades de elementos
        public const string FontSize = "Tamaina:";
        public const string Alignment = "Lerrokatzea:";
        public const string Days = "Egunak:";

        // Botones de acción
        public const string SaveJson = "?? JSON Gorde";
        public const string GenerateQR = "?? QR Sortu JSON-rekin";
        public const string StartServer = "?? Zerbitzaria Hasi";
        public const string StopServer = "?? Zerbitzaria Gelditu";

        // Secciones
        public const string ExportWidget = "?? Widget-a Esportatu";
        public const string HttpServer = "?? HTTP Zerbitzaria (Hautazkoa)";
        public const string RequiresAdmin = "Administratzaile gisa exekutatu behar da";

        // Mensajes
        public const string NoQRCode = "QR koderik ez";
        public const string UseGenerateButton = "Erabili\n'?? QR Sortu JSON-rekin' botoia";
        public const string ScanFromAndroid = "? Android-etik eskaneatu";
        public const string Information = "?? Informazioa:";
        public const string ApproximatePreview = "Widget-aren gutxi gorabeherako aurrebista";

        // Tamaños de widget
        public const string SizeSmall = "Txikia (2x1)";
        public const string SizeSmallDesc = "Informazio oinarrizkoa: tenperatura + ikonoa";
        public const string SizeMedium = "Ertaina (4x2)";
        public const string SizeMediumDesc = "Tenperatura, baldintza eta datu gehigarriak";
        public const string SizeLarge = "Handia (4x3)";
        public const string SizeLargeDesc = "Informazio osoa: eguneko iragarpena barne";
        public const string SizeExtraLarge = "Oso Handia (4x4)";
        public const string SizeExtraLargeDesc = "Informazio osoa + orduko iragarpena";

        // Validaciones
        public const string WarningMaxElements = "?? Gehiegi elementu!";
        public const string WarningMessage = "Widget tamaina honekin {0} elementu gehienez gomendatzen dira.\nUnean {1} dituzu.";
        public const string ElementAdded = "Elementua gehitu da";
        public const string CannotAddElement = "Ezin da elementua gehitu";
        public const string MaxElementsReached = "Widget tamaina honekin gehienezko elementu kopurura iritsi zara ({0}).\n\nGomendapena: hautatu tamaina handiagoa edo kendu elementuak.";

        // Tooltips
        public const string TooltipGenerateQR = "JSON osoa duen QR kode bat sortzen du.\nEz du administratzaile baimenik behar.";
        public const string TooltipStartServer = "HTTP zerbitzari lokala hasten du.\nAdministratzaile baimenak behar ditu.";

        // Mensajes de éxito/error
        public const string QRGenerated = "? QR kodea arrakastaz sortu da!";
        public const string QRGeneratedMessage = "JSON osoa QR kodean txertatuta dago.\nTamaina: {0} karaktere\n\n?? Android gailutik eskaneatu";
        public const string QRError = "? Errorea QR kodea sortzean";
        public const string QRErrorMessage = "? Errorea QR kodea sortzean:\n\n{0}\n\nOharra: JSONa oso handia bada, HTTP zerbitzaria erabili";

        public const string ServerStarted = "Zerbitzaria Aktibo";
        public const string ServerStartedMessage = "? Zerbitzaria hemen hasita:\n{0}\n\n?? Android gailutik QR kodea eskaneatu\n\n?? Baimen arazoak badituzu, Administratzaile gisa exekutatu aplikazioa.";
        
        public const string AccessDenied = "Baimen Errorea";
        public const string AccessDeniedMessage = "? Sarbide ukatua zerbitzaria hastean.\n\nKONPONBIDEAK:\n\n1. Exekutatu aplikazioa Administratzaile gisa (eskuin-klika > Exekutatu administratzaile gisa)\n\n2. Edo hobeto: Erabili '?? QR Sortu JSON-rekin' botoia, ez ditu baimen berezirik behar.\n\nXehetasun teknikoak: {0}";

        public const string ServerStopped = "Zerbitzaria geldituta";
        public const string ServerStoppedMessage = "Zerbitzaria zuzen geldituta";

        public const string JsonSaved = "Widget-a Gorde";
        public const string JsonSavedMessage = "? Widget-a hemen gordeta:\n{0}";
        public const string JsonSaveError = "Errorea";
        public const string JsonSaveErrorMessage = "? Errorea fitxategia gordetzean:\n{0}";

        public const string JsonUpdated = "CliMaster Diseinatzailea";
        public const string JsonUpdatedMessage = "JSONa aurrebista panelean eguneratuta";
    }
}
