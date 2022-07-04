using Structurizr;
using Structurizr.Api;

namespace fas_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 74068;
            const string apiKey = "a4a87d71-4f14-42ee-aab8-0ce43b1ff894";
            const string apiSecret = "411c72e6-eca0-4282-b88a-09818389aeb7";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Vende Hoy Tu Vehiculo", "Sistema de Ventas de Vehículos Usados");
            Model model = workspace.Model;
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SoftwareSystem demoSystem = model.AddSoftwareSystem("Sistema de Ventas de Vehículos Semi-Nuevos", "Permite a los usuarios publicar sus vehiculos semin-nuevos para posteriormente poder venderlos.");
            SoftwareSystem api1 = model.AddSoftwareSystem("SAT (Sistema de Administración Tributaria)", "Permite que el vehículo no tenga ninguna papeleta ni impuesto vehicular pendiente de pago");
            SoftwareSystem api2 = model.AddSoftwareSystem("SUNARP (Superintendencia Nacional de los Registros Públicos)", "Permite la validación de los datos del vehículo");
            SoftwareSystem api3 = model.AddSoftwareSystem("RENIEC", "Permite la validación de los datos biométricos del cliente");
            SoftwareSystem email = model.AddSoftwareSystem("E-MAIL System", "Maneja el sistema de envio de correos electronicos al cliente");

            Person person1 = model.AddPerson("Cliente", "Persona o Empresa que publica la venta del vehículo.");
            Person person2 = model.AddPerson("Administrador", "Experto de la empresa que se encarga de la venta y compra de los vehículos.");
            Person person3 = model.AddPerson("Representante", "Persona que realiza los trabajos legales para la compra y venta de los vehículos.");

            person1.Uses(demoSystem,"Realiza o consulta publicaciones de venta de vehiculos semi-nuevos");
            person2.Uses(demoSystem, "Realiza consutas para visualizar y analizar la compra y venta de los vehivulos, para luego enviar una oferta de compra al cliente");
            person3.Uses(demoSystem, "Realiza la parte legal de una compra o venta de un vehículo");

            demoSystem.Uses(api1, "Consulta si el vehiculo posee papeletas o impuestos pendientes de pago");
            demoSystem.Uses(api2, "Consulta los datos del vehiculo, para poder validarlos");
            demoSystem.Uses(api3, "Consulta los datos biométricos del cliente, para poder validarlos");
            demoSystem.Uses(email, "Envia correos electronicos a los clientes");

            email.Delivers(person1, "Envia correos a los clientes");

            demoSystem.AddTags("demo");
            api1.AddTags("api1");
            api2.AddTags("api2");
            api3.AddTags("api3");
            email.AddTags("email");
            person1.AddTags("person1");
            person2.AddTags("person2");
            person3.AddTags("person3");
            
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("person1") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("person2") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("person3") { Background = "#facc2e", Shape = Shape.Robot });
            styles.Add(new ElementStyle("demo") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("api1") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("api2") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("api3") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("email") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });


            SystemContextView contextView = viewSet.CreateSystemContextView(demoSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            
            // 2. Diagrama de Contenedores
            Container mobileApplication = demoSystem.AddContainer("Mobile App", "Permite a los usuarios visualizar un dashboard con el resumen de los anuncios de la venta de vehiculos para poder cotizarlos.", "Flutter");
            Container webApplication = demoSystem.AddContainer("Web App", "Permite a los usuarios visualizar un dashboard con el resumen de los anuncios de la venta de vehiculos para poder cotizarlos.", "Flutter Web");
            Container landingPage = demoSystem.AddContainer("Landing Page", "", "Flutter Web");
            Container apiGateway = demoSystem.AddContainer("API Gateway", "API Gateway", "Spring Boot port 8080");

            Container bC1 = demoSystem.AddContainer("Bounded Context 1", "Bounded Context del Microservicio de 1","Spring Boot port 8081");
            Container bC2 = demoSystem.AddContainer("Bounded Context 2", "Bounded Context del Microservicio de 2","Spring Boot port 8082");
            Container bC3 = demoSystem.AddContainer("Bounded Context 3", "Bounded Context del Microservicio de 3","Spring Boot port 8083");
            Container bC4 = demoSystem.AddContainer("Bounded Context 4", "Bounded Context del Microservicio de 4","Spring Boot port 8084");
            Container bC5 = demoSystem.AddContainer("Bounded Context 5", "Bounded Context del Microservicio de 5","Spring Boot port 8085");
            
            Container messageBus = demoSystem.AddContainer("Bus de Mensajes en Cluster de Alta Disponibilidad", "Transporte de eventos del dominio.", "RabbitMQ");
            
            Container bC1DataBase = demoSystem.AddContainer("1 Context DB", "","H2");
            Container bC2DataBase = demoSystem.AddContainer("2 Context DB", "","H2");
            Container bC3DataBase = demoSystem.AddContainer("3 Context DB", "","H2");
            Container bC4DataBase = demoSystem.AddContainer("4 Context DB", "","H2");
            Container bC5DataBase = demoSystem.AddContainer("5 Context DB", "","H2"); 
            
            person1.Uses(mobileApplication, "Consulta");
            person1.Uses(webApplication, "Consulta");
            person1.Uses(landingPage, "Consulta");
            
            person2.Uses(mobileApplication, "Consulta");
            person2.Uses(webApplication, "Consulta");
            person2.Uses(landingPage, "Consulta");
            
            person3.Uses(mobileApplication, "Consulta");
            person3.Uses(webApplication, "Consulta");
            person3.Uses(landingPage, "Consulta");
            
            mobileApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            
            apiGateway.Uses(bC1, "API Request", "JSON/HTTPS");
            apiGateway.Uses(bC2, "API Request", "JSON/HTTPS");
            apiGateway.Uses(bC3, "API Request", "JSON/HTTPS");
            apiGateway.Uses(bC4, "API Request", "JSON/HTTPS");
            apiGateway.Uses(bC5, "API Request", "JSON/HTTPS");
            
            bC1.Uses(messageBus, "Publica y consume eventos del dominio");
            bC2.Uses(messageBus, "Publica y consume eventos del dominio");
            bC3.Uses(messageBus, "Publica y consume eventos del dominio");
            bC3.Uses(api1, "Consume");
            bC3.Uses(api2, "Consume");
            bC4.Uses(messageBus, "Publica y consume eventos del dominio");
            bC5.Uses(messageBus, "Publica y consume eventos del dominio");
            bC5.Uses(api3, "Consume");

            bC1.Uses(bC1DataBase, "","JDBC");
            bC2.Uses(bC2DataBase, "","JDBC");
            bC3.Uses(bC3DataBase, "","JDBC");
            bC4.Uses(bC4DataBase, "","JDBC");
            bC5.Uses(bC5DataBase, "","JDBC");
            
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");
            messageBus.AddTags("MessageBus");
            
            bC1.AddTags("BC1");
            bC2.AddTags("BC2");
            bC3.AddTags("BC3");
            bC4.AddTags("BC4");
            bC5.AddTags("BC5");
            
            bC1DataBase.AddTags("DB1");
            bC2DataBase.AddTags("DB2");
            bC3DataBase.AddTags("DB3");
            bC4DataBase.AddTags("DB4");
            bC5DataBase.AddTags("DB5"); 

            styles.Add(new ElementStyle("MobileApp") { Background = "#009688", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#009688", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("MessageBus") { Width = 850, Background = "#fd8208", Color = "#ffffff", Shape = Shape.Pipe, Icon = "" });
            
            styles.Add(new ElementStyle("BC1") { Shape = Shape.Hexagon, Background = "#FF4081", Icon = "" });
            styles.Add(new ElementStyle("BC2") { Shape = Shape.Hexagon, Background = "#FF4081", Icon = "" });
            styles.Add(new ElementStyle("BC3") { Shape = Shape.Hexagon, Background = "#FF4081", Icon = "" });
            styles.Add(new ElementStyle("BC4") { Shape = Shape.Hexagon, Background = "#FF4081", Icon = "" });
            styles.Add(new ElementStyle("BC5") { Shape = Shape.Hexagon, Background = "#FF4081", Icon = "" });
            
            styles.Add(new ElementStyle("DB1") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("DB2") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("DB3") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("DB4") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("DB5") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            
            ContainerView containerView = viewSet.CreateContainerView(demoSystem, "Contenedor", "Diagrama de contenedores");
            containerView.PaperSize = PaperSize.A2_Landscape;
            containerView.AddAllElements();
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}