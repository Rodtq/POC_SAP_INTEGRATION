# POC_SAP_INTEGRATION
Proof of concept for .net and SAP integration, retrieving values form SAP
Olá,

Neste documento discutiremos alguns pontos sobre conexão entre o SAP e  a plataforma .Net utilizando C#.
Primeiro passo para a conexão com o SAP é saber os parametros de configuração corretos para conexão. Estes parâmetros poderão ficar armazenados no web.config como mostra o xml exemplo abaixo.

<configuration>

  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    <!--Name to identify your SAP system by your code-->
    <!--<add key="SAP_NAME" value="DEV"/>-->
    <add key="SAP_NAME" value="MRQ"/>
    <add key="SAP_USERNAME" value="AZRMA"/>
    <add key="SAP_PASSWORD" value="Sbart01"/>
    <add key="SAP_CLIENT" value="100"/>
    <add key="SAP_APPSERVERHOST" value="10.16.40.41"/>
    <add key="SAP_SYSTEMNUM" value="01"/>
    <add key="SAP_LANGUAGE" value="EN"/>
    <add key="SAP_POOLSIZE" value="10"/>
    
  </appSettings>
.
.	
.
</configuration>

Logo após que as configurações no web config estiverem corretas, deveremos criar uma classe para ler as configurações e aplicá-las junto à biblioteca spanco v3.0 Esta calsse se encontra em App_Start/SapDestinationConfig.cs

Para a inicialização ser satisfatória, uma chamada será feita a partir da clase Global.asax pelo metodo estatico SapDestinationConfig.RegisterDestinations(); desta maneira teremos as configurações prontas quando uma chamada REST ocorrer.
Esta chamada REST será interceptada pelo controller o qual terá uma instância da classe SapManager.cs, esta classe é a responsável por executar a conexão com o SAP recuperar os modelos via RFC e popular os modelos com informações oriundas do SAP.A partir do modelo entregue da RFC utilizada como exemplo teremos que preencher os requisitos para os campos mandatórios da RFC

RFC_NAME: ZBR_GET_SALES_DATA

Parameter / Field Name	Key	Data Element	Data Type	Length	Decimals	Description	Obs
Import	 	 	 	 	 	 	 
I_BUKRS		BUKRS	CHAR	4	0	Company Code	Mandatory
I_FKDAT							Mandatory
SIGN			CHAR	1		Inclusion/exclusion criterion SIGN for range tables	I (Inclusive) / E (Exclusive)
OPTION			CHAR	2		Selection operator OPTION for range tables	EQ (Equal) / BT (Between)
LOW		FKDAT	DATS	8	0	Start Period (YYYYMMDD)	Mandatory
HIGH		FKDAT	DATS	8	0	End Period (YYYYMMDD)	Optional
I_VBELN						Billing Document	Optional
SIGN			CHAR	1		Inclusion/exclusion criterion SIGN for range tables	I (Inclusive) / E (Exclusive)
OPTION			CHAR	2		Selection operator OPTION for range tables	EQ (Equal) / BT (Between)
LOW		VBELN_VF	CHAR	10		Billing Document	
HIGH		VBELN_VF	CHAR	10		Billing Document	
I_KUNAG						Sold-to party	Optional
SIGN			CHAR	1		Inclusion/exclusion criterion SIGN for range tables	I (Inclusive) / E (Exclusive)
OPTION			CHAR	2		Selection operator OPTION for range tables	EQ (Equal) / BT (Between)
LOW		KUNAG	CHAR	10	0	Sold-to party	
HIGH		KUNAG	CHAR	10	0	Sold-to party	


Após o preenchimento correto dos parametros a partir do metodo Invoke, conseguiremos resgatar valores no SAP, podendo assim transferir dados e preencher os DatSets para gravação no banco.

Nome da biblioteca: SAPDotNetConnector3

Documentação oficial: https://help.sap.com/saphelp_crm700_ehp02/helpdata/EN/4a/097b0543f4088ce10000000a421937/content.htm

Instalação: Instalação via nugget, gerenciador de dependencias do visual Studio.
