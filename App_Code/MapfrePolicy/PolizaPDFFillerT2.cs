using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Protec.Authentication.Membership;

/// <summary>
/// Summary description for PolizaPDFFillerT2
/// </summary>
public static class PolizaPDFFillerT2
{
    private static decimal getExchangeRate()
    {
        UserInformation user = (UserInformation)HttpContext.Current.User.Identity;
        return user.ExchangeRate;
    }
    #region TextoAclaraciones
    private static string[][] textAclaracionesFP = new string[][] {
        new string[] { 
            "EL PRESENTE CONTRATO HA SIDO EXPEDIDO EN MONEDA EXTRANJERA A SOLICITUD DEL ASEGURADO Y QUEDARÁ SUJETO",
            "A LO DISPUESTO EN LA LEY GENERAL DE INSTITUCIONES Y SOCIEDADES MUTUALISTAS DE SEGURO, A LAS REGLAS QUE",
            "SE CITAN A CONTINUACIÓN Y A LAS DEMÁS DISPOSICIONES LEGALES Y ADMINISTRATIVES QUE RESULTEN APLICABLES."
        },
        new string[] {
            "TANTO LAS SUMAS ASEGURADAS, COMO LAS PRIMAS DE ESTE SEGURO, SE HAN ESTABLECIDO EN MONEDA EXTRANJERA, CUYA",
            "DESCRIPCIÓN APARECE EN LA CARÁTULA DE LA PÓLIZA EN EL APARTADO CORRESPONDIENTE, SIN EMBARGO LAS",
            "INDEMNIZACIONES EN CASO DE SINIESTRO QUE SE REALICEN EN EL TERRITORIO MEXICANO, DEBERÁN HACERSE EN LOS",
            "TÉRMINOS DISPUESTOS EN LA LEY MONETARIA VIGENTE AL MOMENTO DEL SINIESTRO."
        },
        new string[] {
            "EL PAGO DE PRIMAS, INDEMNIZACIONES EN CASO DE SINIESTRO PROCEDENTE, COMISIONES (EXCEPTO LAS QUE PERCIBAN",
            "LOS AGENTES DE SEGUROS AUTORIZADOS PARA ACTUAR EN EL PAÍS) Y DEMÁS ELEMENTOS RELACIONADOS CON LO ANTERIOR,",
            "SE HARÁN EN EL EXTRANJERO INVARIABLEMENTE EN MONEDA EXTRANJERA POR CONDUCTO DE LAS INSTITUCIONES DE ",
            "CRÉDITO MEXICANAS CUANDO ESTA COMPAÑÍA NO CUENTE CON OFICINAS EN EL EXTERIOR."
        },
        new string[] {
            "CUANDO EL EVENTO DAÑOSO OCURRA EN TERRITORIO MEXICANO Y QUE SE ENCUENTRE CUBIERTO CON RESPONSABILIDAD",
            "CIVIL LA INDEMNIZACIÓN SE DEBERÁ CUBRIR EN MONEDA NACIONAL, AÚN CUANDO LA PRIMA SEA PAGADA EN DIVISAS,",
            "SIEMPRE QUE EL TITULAR DEL DERECHO DERIVADO DE LA COBERTURA DE ESTA PÓLIZA ESTÉ DOMICILIADO EN LA",
            "REPÚBLICA MEXICANA."
        },
        new string[] {
            "EL ASEGURADO Y LA COMPAÑÍA CONVIENEN Y ACEPTAN QUE PARA CONOCER Y RESOLVER CONTROVERSIAS DERIVADAS DEL",
            "PRESENTE CONTRATO DE SEGURO A QUE SE REFIEREN LAS PRESENTES REGLAS, SERÁN COMPETENTES LAS AUTORIDADES",
            "MEXICANAS EN LOS TÉRMINOS DEL ARTÍCULO 135 DE LA LEY GENERAL DE INSTITUCIONES Y SOCIEDADES MUTUALISTAS",
            "DE SEGUROS, SIN PERJUICIO DE QUE LAS COBERTURAS EN QUE CONFORME A LOS USOS INTERNACIONALES SE ACOSTUMBRE",
            "ESTABLECERLO ASÍ, SE SEÑALE LA POSIBILIDAD DE COMPETENCIA CONCURRENTE."
        }
    };

    private static string[][] textPuntoA = new string[][] {
        new string[] {
            "ESTA PÓLIZA NO ASEGURA PÉRDIDAS, DAÑOS, DESTRUCCIÓN, DISTORSIÓN, BORRADO, CORRUPCIÓN O ALTERACIÓN",
            "DE DATOS ELECTRÓNICOS DERIVADOS DE CUALQUIER CAUSA QUE SEA, (INCLUYENDO PERO NO LIMITADO A VIRUS",
            "DE COMPUTADORA) O PÉRDIDA DE USO, REDUCCIÓN EN FUNCIONALIDAD, COSTOS, GASTOS DE CUALQUIER NATURALEZA",
            "RESULTANTE, INDEPENDIENTEMENTE, DE CUALQUIER OTRA CAUSA O EVENTO CON CONTRIBUYA AL SINIESTRO,",
            "YA SEA SIMULTÁNEAMENTE O EN CUALQUIER ORDEN DE PÉRDIDAS:"
        },
        new string[] {
            "DATOS ELECTRÓNICOS SIGNIFICA: CONCEPTOS E INFORMACIÓN CONVERTIDA A UNA FORMA UTILIZABLE DE COMUNICACIÓN,",
            "INTERPRETACIÓN O PROCESAMIENTO ELECTRÓNICO O ELECTROMECÁNICO O EQUIPOS CONTROLADOS ELECTRÓNICAMENTE",
            "INCLUYENDO SUS PROGRAMAS, \"SOFTWARE\" Y OTRAS INSTRUCCIONES CODIFICADAS PARA PROCESAMIENTO Y MANIPULACIÓN",
            "DE DATOS O EL MANEJO O MANIPULACIÓN DE TALES EQUIPOS."
        },
        new string[] {
            "VIRUS DE COMPUTADORA SIGNIFICA: CUALQUIER ALTERACIÓN DAÑINA CON INSTRUCCIONES NO AUTORIZADAS O CÓDIGOS",
            "NO AUTORIZADOS INTRODUCIDOS MALICIOSAMENTE EN PROGRAMAS DE CÓMPUTO DE CUALQUIER TIPO, QUE SE PROPAGUEN",
            "POR SÍ MISMOS O DE CUALQUIER OTRA FORMA, A TRAVÉS DE SISTEMAS DE CÓMPUTO O REDES DE CUALQUIER NATURALEZA,",
            "VIRUS DE COMPUTADORA INCLUYE PERO NO SE LIMITA A \"TROJAN HORSES,\" \"WORMS\" Y \"TIME LOGIC BOMBS.\""
        }
    };
    private static string[][] textPuntoB = new string[][] {
        new string[] {
            "SIN EMBARGO, EN EL CASO DE CUALQUIERA DE LOS RIESGOS AMPARADOS EN ESTA PÓLIZA QUE RESULTE DE CUALQUIER",
            "PUNTO DESCRITO EN EL PARRAFO A) ANTERIOR, ESTA PÓLIZA, SUJETA A TODOS SUS TÉRMINOS, CONDICIONES Y",
            "EXCLUSIONES, CUBRIRÁ CUALQUIER DAÑO FÍSICO A LA PROPIEDAD ASEGURADA DURANTE EL PERIODO DE LA PÓLIZA,",
            "SIEMPRE QUE DICHO DAÑO SEA OBJETO DE COBERTURA DE ESTA PÓLIZA."
        }
    };

    private static string[][] textAclaracionesSP = new string[][] {
        new string[] {
            "SIN PERJUICIO DE CUALQUIER DISPOSICIÓN CONTRARIA DENTRO DE ESTA PÓLIZA O CUALQUIER ANEXO AL MISMO, POR LA",
            "PRESENTE SE ACUERDA Y SE ENTIENDE LO SIGUIENTE:"
        }, 
        new string[] {
            "SI LOS MEDIOS DE PROCESAMIENTO ELECTRÓNICO DE DATOS ASEGURADOS POR LA PÓLIZA SUFREN PÉRDIDA O DAÑOS FÍSICOS",
            "ASEGURADOS POR ESTA PÓLIZA, ENTONCES, LA BASE DE VALUACIÓN DEBERÁ SER EL COSTO DEL MEDIO EN BLANCO, MÁS EL",
            "COSTO DE COPIADO DE LOS DATOS ELECTRÓNICOS DESDE SU GENERACIÓN ORIGINAL PREVIA, ESTOS COSTOS NO INCLUIRÁN",
            "INVESTIGACIÓN O INGENIERÍA NI NINGÚN COSTO DE RECREACIÓN, RECOLECCIÓN O ENSAMBLAJE DE TALES DATOS ELECTRÓNICOS.",
            "SI EL MEDIO NO ES REPARADO,REEMPLAZADO O RESTAURADO, LA BASE DE VALUACIÓN DEBERÁ SER EL COSTO DEL MEDIO ",
            "EN BLANCO."
        },
        new string[] {
            "SIN EMBARGO, ESTA PÓLIZA NO AMPARA NINGUNA CANTIDAD REFERIDA AL VALOR DE TALES DATOS ELECTRÓNICOS PARA EL ",
            "ASEGURADO O CUALQUIER OTRA PARTE QUE PUDIERA RESULTAR ASEGURADA, AUN SI TALES DATOS ELECTRÓNICOS NO PUEDEN",
            "SER RECREADOS, RECOLECTADOS O ENSAMBLADOS."
        }
    };

    private static string[] textCondicionesAplicables = new string[] {
        "CONDICIONES GENERALES FORMA DAHO-001-C",
        "ENDOSO EN MONEDA EXTRANJERA",
        "ENDOSO DE FENÓMENOS METEOROLÓGICOS",
        "ENDOSO DE TERREMOTO Y/O ERUPCIÓN VOLCÁNICA",
        "ENDOSO DE EXCLUSIÓN DE RIESGOS CIBERNÉTICOS"
    };
    #endregion

    #region TextoEndosoCondicionesGenerales

    private static string[][] textEndosoCondicionesGenerales = new string[][] {
        new string[] {
        "POR ESTE MEDIO SE LE INFORMA QUE CON LA ENTRADA EN VIGOR DE LA LEY DE INSTITUCIONES DE SEGUROS  Y DE FIANZAS ",
        "Y LA CIRCULAR ÚNICA DE SEGUROS Y FIANZAS EL DÍA 04 DE ABRIL DE 2015; MEDIANTE ESTE ENDOSO, SE HACE DE SU",
        "CONOCIMIENTO QUE ALGUNAS CLÁUSULAS Y/O LEYENDAS CONTENIDAS EN LAS CONDICIONES GENERALES DEL PRESENTE",
        "CONTRATO DE SEGURO, SE MODIFICAN, Y/ O ADICIONAN DE CONFORMIDAD CON LOS SIGUIENTES TÉRMINOS:"
        },
        new string[] {
        "(ANTES ARTÍCULO 135 BIS LEY GENERAL DE INSTITUCIONES Y SOCIEDADES MUTUALISTAS DE SEGUROS, ACTUALMENTE CAMBIA A",
        "276 LEY INSTITUCIONES DE SEGUROS Y FIANZAS)",
        "ARTÍCULO 276.- SI UNA INSTITUCIÓN DE SEGUROS NO CUMPLE CON LAS OBLIGACIONES ASUMIDAS EN EL CONTRATO DE SEGURO ",
        "DENTRO DE LOS PLAZOS CON QUE CUENTE LEGALMENTE PARA SU CUMPLIMIENTO, DEBERÁ PAGAR AL ACREEDOR UNA",
        "INDEMNIZACIÓN POR MORA DE ACUERDO CON LO SIGUIENTE:"
        },
        new string[] {
        "I. LAS OBLIGACIONES EN MONEDA NACIONAL SE DENOMINARÁN EN UNIDADES DE INVERSIÓN, AL VALOR DE ÉSTAS EN LA FECHA",
        "DEL VENCIMIENTO DE LOS PLAZOS REFERIDOS EN LA PARTE INICIAL DE ESTE ARTÍCULO Y SU PAGO SE HARÁ EN MONEDA",
        "NACIONAL, AL VALOR QUE LAS UNIDADES DE INVERSIÓN TENGAN A LA FECHA EN QUE SE EFECTÚE EL MISMO, DE CONFORMIDAD",
        "CON LO DISPUESTO EN EL PÁRRAFO SEGUNDO DE LA FRACCIÓN VIII DE ESTE ARTÍCULO."
        },
        new string[] {
        "ADEMÁS, LA INSTITUCIÓN DE SEGUROS PAGARÁ UN INTERÉS MORATORIO SOBRE LA OBLIGACIÓN DENOMINADA EN UNIDADES DE",
        "INVERSIÓN CONFORME A LO DISPUESTO EN EL PÁRRAFO ANTERIOR, EL CUAL SE CAPITALIZARÁ MENSUALMENTE Y CUYA TASA",
        "SERÁ IGUAL AL RESULTADO DE MULTIPLICAR POR 1.25 EL COSTO DE CAPTACIÓN A PLAZO DE PASIVOS DENOMINADOS EN",
        "UNIDADES DE INVERSIÓN DE LAS INSTITUCIONES DE BANCA MÚLTIPLE DEL PAÍS, PUBLICADO POR EL BANCO DE MÉXICO EN EL",
        "DIARIO OFICIAL DE LA FEDERACIÓN, CORRESPONDIENTE A CADA UNO DE LOS MESES EN QUE EXISTA MORA;"
        },
        new string[] {
        "II. CUANDO LA OBLIGACIÓN PRINCIPAL SE DENOMINE EN MONEDA EXTRANJERA, ADICIONALMENTE AL PAGO DE ESA OBLIGACIÓN,",
        "LA INSTITUCIÓN DE SEGUROS ESTARÁ OBLIGADA A PAGAR UN INTERÉS MORATORIO EL CUAL SE CAPITALIZARÁ MENSUALMENTE Y",
        "SE CALCULARÁ APLICANDO AL MONTO DE LA PROPIA OBLIGACIÓN, EL PORCENTAJE QUE RESULTE DE MULTIPLICAR POR 1.25 EL",
        "COSTO DE CAPTACIÓN A PLAZO DE PASIVOS DENOMINADOS EN DÓLARES DE LOS ESTADOS UNIDOS DE AMÉRICA, DE LAS",
        "INSTITUCIONES DE BANCA MÚLTIPLE DEL PAÍS, PUBLICADO POR EL BANCO DE MÉXICO EN EL DIARIO OFICIAL DE LA",
        "FEDERACIÓN CORRESPONDIENTE A CADA UNO DE LOS MESES EN QUE EXISTA MORA;"
        },
        new string[] {
        "III. EN CASO DE QUE A LA FECHA EN QUE SE REALICE EL CÁLCULO NO SE HAYAN PUBLICADO LAS TASAS DE REFERENCIA PARA",
        "EL CÁLCULO DEL INTERÉS MORATORIO A QUE ALUDEN LAS FRACCIONES I Y II DE ESTE ARTÍCULO, SE APLICARÁ LA DEL MES",
        "INMEDIATO ANTERIOR Y, PARA EL CASO DE QUE NO SE PUBLIQUEN DICHAS TASAS, EL INTERÉS MORATORIO SE COMPUTARÁ",
        "MULTIPLICANDO POR 1.25 LA TASA QUE LAS SUSTITUYA, CONFORME A LAS DISPOSICIONES APLICABLES;"
        },
        new string[] {
        "IV. LOS INTERESES MORATORIOS A QUE SE REFIERE ESTE ARTÍCULO SE GENERARÁN POR DÍA, A PARTIR DE LA FECHA DEL",
        "VENCIMIENTO DE LOS PLAZOS REFERIDOS EN LA PARTE INICIAL DE ESTE ARTÍCULO Y HASTA EL DÍA EN QUE SE EFECTÚE EL",
        "PAGO PREVISTO EN EL PÁRRAFO SEGUNDO DE LA FRACCIÓN VIII DE ESTE ARTÍCULO. PARA SU CÁLCULO, LAS TASAS DE",
        "REFERENCIA A QUE SE REFIERE ESTE ARTÍCULO DEBERÁN DIVIDIRSE ENTRE TRESCIENTOS SESENTA Y CINCO Y MULTIPLICAR",
        "EL RESULTADO POR EL NÚMERO DE DÍAS CORRESPONDIENTES A LOS MESES EN QUE PERSISTA EL INCUMPLIMIENTO;"
        },
        new string[] {
        "V. EN CASO DE REPARACIÓN O REPOSICIÓN DEL OBJETO SINIESTRADO, LA INDEMNIZACIÓN POR MORA CONSISTIRÁ ÚNICAMENTE",
        "EN EL PAGO DEL INTERÉS CORRESPONDIENTE A LA MONEDA EN QUE SE HAYA DENOMINADO LA OBLIGACIÓN PRINCIPAL CONFORME",
        "A LAS FRACCIONES I Y II DE ESTE ARTÍCULO Y SE CALCULARÁ SOBRE EL IMPORTE DEL COSTO DE LA REPARACIÓN O",
        "REPOSICIÓN;"
        },
        new string[] {
        "VI. SON IRRENUNCIABLES LOS DERECHOS DEL ACREEDOR A LAS PRESTACIONES INDEMNIZATORIAS ESTABLECIDAS EN ESTE",
        "ARTÍCULO. EL PACTO QUE PRETENDA EXTINGUIRLOS O REDUCIRLOS NO SURTIRÁ EFECTO LEGAL ALGUNO. ESTOS DERECHOS",
        "SURGIRÁN POR EL SOLO TRANSCURSO DEL PLAZO ESTABLECIDO POR LA LEY PARA EL PAGO DE LA OBLIGACIÓN PRINCIPAL,",
        "AUNQUE ÉSTA NO SEA LÍQUIDA EN ESE MOMENTO."
        },
        new string[] {
        "UNA VEZ FIJADO EL MONTO DE LA OBLIGACIÓN PRINCIPAL CONFORME A LO PACTADO POR LAS PARTES O EN LA RESOLUCIÓN",
        "DEFINITIVA DICTADA EN JUICIO ANTE EL JUEZ O ÁRBITRO, LAS PRESTACIONES INDEMNIZATORIAS ESTABLECIDAS EN ESTE",
        "ARTÍCULO DEBERÁN SER CUBIERTAS POR LA INSTITUCIÓN DE SEGUROS SOBRE EL MONTO DE LA OBLIGACIÓN PRINCIPAL ASÍ",
        "DETERMINADO;"
        },
        new string[] {
        "VII. SI EN EL JUICIO RESPECTIVO RESULTA PROCEDENTE LA RECLAMACIÓN, AUN CUANDO NO SE HUBIERE DEMANDADO EL PAGO",
        "DE LA INDEMNIZACIÓN POR MORA ESTABLECIDA EN ESTE ARTÍCULO, EL JUEZ O ÁRBITRO, ADEMÁS DE LA OBLIGACIÓN",
        "PRINCIPAL, DEBERÁ CONDENAR AL DEUDOR A QUE TAMBIÉN CUBRA ESAS PRESTACIONES CONFORME A LAS FRACCIONES",
        "PRECEDENTES;"
        },
        new string[] {
        "VIII. LA INDEMNIZACIÓN POR MORA CONSISTENTE EN EL SISTEMA DE ACTUALIZACIÓN E INTERESES A QUE SE REFIEREN LAS",
        "FRACCIONES I, II, III Y IV DEL PRESENTE ARTÍCULO SERÁ APLICABLE EN TODO TIPO DE SEGUROS, SALVO TRATÁNDOSE DE",
        "SEGUROS DE CAUCIÓN QUE GARANTICEN INDEMNIZACIONES RELACIONADAS CON EL IMPAGO DE CRÉDITOS FISCALES,",
        "EN CUYO CASO SE ESTARÁ A LO DISPUESTO POR EL CÓDIGO FISCAL DE LA FEDERACIÓN."
        },
        new string[] {
        "EL PAGO QUE REALICE LA INSTITUCIÓN DE SEGUROS SE HARÁ EN UNA SOLA EXHIBICIÓN QUE COMPRENDA EL SALDO TOTAL POR",
        "LOS SIGUIENTES CONCEPTOS:",
        "A)       LOS INTERESES MORATORIOS;",
        "B)       LA ACTUALIZACIÓN A QUE SE REFIERE EL PRIMER PÁRRAFO DE LA FRACCIÓN I DE ESTE ARTÍCULO, Y",
        "C)       LA OBLIGACIÓN PRINCIPAL.",
        "EN CASO DE QUE LA INSTITUCIÓN DE SEGUROS NO PAGUE EN UNA SOLA EXHIBICIÓN LA TOTALIDAD DE LOS IMPORTES DE LAS",
        "OBLIGACIONES ASUMIDAS EN EL CONTRATO DE SEGUROS Y LA INDEMNIZACIÓN POR MORA, LOS PAGOS QUE REALICE SE",
        "APLICARÁN A LOS CONCEPTOS SEÑALADOS EN EL ORDEN ESTABLECIDO EN EL PÁRRAFO ANTERIOR, POR LO QUE LA",
        "INDEMNIZACIÓN POR MORA SE CONTINUARÁ GENERANDO EN TÉRMINOS DEL PRESENTE ARTÍCULO, SOBRE EL MONTO DE LA",
        "OBLIGACIÓN PRINCIPAL NO PAGADA, HASTA EN TANTO SE CUBRA EN SU TOTALIDAD."
        },
        new string[] {
        "CUANDO LA INSTITUCIÓN INTERPONGA UN MEDIO DE DEFENSA QUE SUSPENDA EL PROCEDIMIENTO DE EJECUCIÓN PREVISTO EN",
        "ESTA LEY, Y SE DICTE SENTENCIA FIRME POR LA QUE QUEDEN SUBSISTENTES LOS ACTOS IMPUGNADOS, EL PAGO O COBRO",
        "CORRESPONDIENTES DEBERÁN INCLUIR LA INDEMNIZACIÓN POR MORA QUE HASTA ESE MOMENTO HUBIERE GENERADO LA",
        "OBLIGACIÓN PRINCIPAL, Y"
        },
        new string[] {
        "IX. SI LA INSTITUCIÓN DE SEGUROS, DENTRO DE LOS PLAZOS Y TÉRMINOS LEGALES, NO EFECTÚA EL PAGO DE LAS",
        "INDEMNIZACIONES POR MORA, EL JUEZ O LA COMISIÓN NACIONAL PARA LA PROTECCIÓN Y DEFENSA DE LOS USUARIOS DE",
        "SERVICIOS FINANCIEROS, SEGÚN CORRESPONDA, LE IMPONDRÁN UNA MULTA DE 1000 A 15000 DÍAS DE SALARIO."
        },
        new string[] {
        "EN EL CASO DEL PROCEDIMIENTO ADMINISTRATIVO DE EJECUCIÓN PREVISTO EN EL ARTÍCULO 278 DE ESTA LEY, SI LA",
        "INSTITUCIÓN DE SEGUROS, DENTRO DE LOS PLAZOS O TÉRMINOS LEGALES, NO EFECTÚAN EL PAGO DE LAS INDEMNIZACIONES",
        "POR MORA, LA COMISIÓN LE IMPONDRÁ LA MULTA SEÑALADA EN ESTA FRACCIÓN, A PETICIÓN DE LA AUTORIDAD EJECUTORA QUE",
        "CORRESPONDA CONFORME A LA FRACCIÓN II DE DICHO ARTÍCULO."
        },
        new string[] {
        "2.- COMPETENCIA ",
        "(ANTES ARTÍCULO 136 BIS LEY GENERAL DE INSTITUCIONES Y SOCIEDADES MUTUALISTAS DE SEGUROS, ACTUALMENTE CAMBIA",
        "A 277 LEY INSTITUCIONES DE SEGUROS Y FIANZAS)"
        },
        new string[] {
        "ARTÍCULO 277.- EN MATERIA JURISDICCIONAL PARA EL CUMPLIMIENTO DE LA SENTENCIA EJECUTORIADA QUE SE DICTE EN EL",
        "PROCEDIMIENTO, EL JUEZ DE LOS AUTOS REQUERIRÁ A LA INSTITUCIÓN DE SEGUROS, SI HUBIERE SIDO CONDENADA, PARA QUE",
        "COMPRUEBE DENTRO DE LAS SETENTA Y DOS HORAS SIGUIENTES, HABER PAGADO LAS PRESTACIONES A QUE HUBIERE SIDO",
        "CONDENADA Y EN CASO DE OMITIR LA COMPROBACIÓN, EL JUEZ ORDENE AL INTERMEDIARIO DEL MERCADO DE VALORES O A LA",
        "INSTITUCIÓN DEPOSITARIA DE LOS VALORES DE LA INSTITUCIÓN DE SEGUROS QUE, SIN RESPONSABILIDAD PARA LA",
        "INSTITUCIÓN DEPOSITARIA Y SIN REQUERIR EL CONSENTIMIENTO DE LA INSTITUCIÓN DE SEGUROS, EFECTÚE EL REMATE DE",
        "VALORES PROPIEDAD DE LA INSTITUCIÓN DE SEGUROS, O, TRATÁNDOSE DE INSTITUCIONES PARA EL DEPÓSITO DE VALORES A",
        "QUE SE REFIERE LA LEY DEL MERCADO DE VALORES, TRANSFIERA LOS VALORES A UN INTERMEDIARIO DEL MERCADO DE VALORES",
        "PARA QUE ÉSTE EFECTÚE DICHO REMATE."
        },
        new string[] {
        "EN LOS CONTRATOS QUE CELEBREN LAS INSTITUCIONES DE SEGUROS PARA LA ADMINISTRACIÓN, INTERMEDIACIÓN, DEPÓSITO O",
        "CUSTODIA DE TÍTULOS O VALORES QUE FORMEN PARTE DE SU ACTIVO, DEBERÁ ESTABLECERSE LA OBLIGACIÓN DEL",
        "INTERMEDIARIO DEL MERCADO DE VALORES O DE LA INSTITUCIÓN DEPOSITARIA DE DAR CUMPLIMIENTO A LO PREVISTO EN EL",
        "PÁRRAFO ANTERIOR."
        },
        new string[] {
        "TRATÁNDOSE DE LOS CONTRATOS QUE CELEBREN LAS INSTITUCIONES DE SEGUROS CON INSTITUCIONES DEPOSITARIAS DE",
        "VALORES, DEBERÁ PREVERSE EL INTERMEDIARIO DEL MERCADO DE VALORES AL QUE LA INSTITUCIÓN DEPOSITARIA DEBERÁ",
        "TRANSFERIR LOS VALORES PARA DAR CUMPLIMIENTO A LO SEÑALADO EN EL PÁRRAFO ANTERIOR Y CON EL QUE LA INSTITUCIÓN",
        "DE SEGUROS DEBERÁ TENER CELEBRADO UN CONTRATO EN EL QUE SE ESTABLEZCA LA OBLIGACIÓN DE REMATAR VALORES PARA",
        "DAR CUMPLIMIENTO A LO PREVISTO EN ESTE ARTÍCULO."
        },
        new string[] {
        "LOS INTERMEDIARIOS DEL MERCADO DE VALORES Y LAS INSTITUCIONES DEPOSITARIAS DE LOS VALORES CON LOS QUE LAS",
        "INSTITUCIONES DE SEGUROS TENGAN CELEBRADOS CONTRATOS PARA LA ADMINISTRACIÓN, INTERMEDIACIÓN, DEPÓSITO O",
        "CUSTODIA DE TÍTULOS O VALORES QUE FORMEN PARTE DE SU ACTIVO, QUEDARÁN SUJETOS, EN CUANTO A LO SEÑALADO EN EL",
        "PRESENTE ARTÍCULO, A LO DISPUESTO EN ESTA LEY Y A LAS DEMÁS DISPOSICIONES APLICABLES."
        },
        new string[] {
        "LA COMPETENCIA POR TERRITORIO PARA DEMANDAR EN MATERIA DE SEGUROS SERÁ DETERMINADA, A ELECCIÓN DEL",
        "RECLAMANTE, EN RAZÓN DEL DOMICILIO DE CUALQUIERA DE LAS DELEGACIONES DE LA COMISIÓN NACIONAL PARA LA",
        "PROTECCIÓN Y DEFENSA DE LOS USUARIOS DE SERVICIOS FINANCIEROS. ASIMISMO, SERÁ COMPETENTE EL JUEZ DEL DOMICILIO",
        "DE DICHA DELEGACIÓN; CUALQUIER PACTO QUE SE ESTIPULE CONTRARIO A LO DISPUESTO EN ESTE PÁRRAFO, SERÁ NULO."
        },
        new string[] {
        "3.- LEYENDA DE REGISTRO",
        "“EN CUMPLIMIENTO A LO DISPUESTO EN EL ARTÍCULO 202 DE LA LEY DE INSTITUCIONES DE SEGUROS Y DE",
        "FIANZAS, LA DOCUMENTACIÓN CONTRACTUAL Y LA NOTA TÉCNICA QUE INTEGRAN ESTE PRODUCTO DE SEGURO,",
        "QUEDARON REGISTRADAS ANTE LA COMISIÓN NACIONAL DE SEGUROS Y FIANZAS, A PARTIR DEL DÍA 01/ 04/ 2015, ",
        "CON EL NÚMERO RESP-S0041-0231-2015”"
        },
        new string[] {
        "LO ANTERIOR SE  REALIZA DE CONFORMIDAD  CON EL ARTÍCULO 19 DE LA LEY SOBE EL CONTRATO DE SEGURO, ADICIONAL",
        "SE RESALTA  QUE LAS PRESENTES MODIFICACIONES Y ADICIONES SEÑALADAS EN ESTE DOCUMENTO, NO AFECTAN LOS DERECHOS",
        "Y OBLIGACIONES ADQUIRIDOS POR LAS PARTES, EN VIRTUD DEL CONTRATO DE SEGURO QUE SE CELEBRA, ÚNICAMENTE TIENEN",
        "COMO  FINALIDAD HACER DE SU CONOCIMIENTOS LAS DISPOSICIONES LEGALES  AQUÍ CONTENIDAS."
        }
    };
    #endregion

    #region TextoEndosoCondicionesGenerales

    private static string[][] textAvisoDePrivacidad = new string[][] {
        new string[] {
        "MAPFRE MÉXICO, S.A., UBICADA EN AVENINAD REVOLUCIÓN 507, COLONIA SAN PEDRO DE LOS PINOS  Y DELEGACIÓN",
        "BENITO JUÁREZ, CIUDAD DE MÉXICO, C.P. 03800 HACE DE SU CONOCIMIENTO QUE SUS DATOS PERSONALES",
        "RECABADOS, QUE SE RECABEN O SE GENEREN CON MOTIVO DE LA RELACIÓN JURÍDICA QUE SE TENGA CELEBRADA, O",
        "QUE EN SU CASO, SE TRATARÁN PARA TODOS LOS FINES VINCULADOS CON LA MENCIONADA RELACIÓN JURÍDICA Y LAS",
        "OBLIGACIONES DERIVADAS DE LA MISMA. EN ESPECÍFICO PARA:"
        },
        new string[] {
        "VISITANTES EN GENERAL, POR RAZONES DE SEGURIDAD Y VIGILANCIA: CUANDO SEA NECESARIO PARA",
        "PROTECCIÓN DE LOS BIENES MUEBLES E INMUEBLES DONDE RESIDE EL DOMICILIO Y LAS OFICINAS DEL RESPONSABLE,",
        "ASÍ COMO PARA PROTEGER TAMBIÉN A LAS PERSONAS Y SUS PERTENENCIAS, CUANDO ACCEDEN A DICHAS OFICINAS."
        },
        new string[] {
        "RECURSOS HUMANOS (CANDIDATOS, EMPLEADOS Y EX EMPLEADOS) SE UTILIZARÁN PARA TODOS LOS FINES",
        "VINCULADOS CON LA RELACIÓN LABORAL, EN ESPECIAL PARA SELECCIÓN, PARA VERIFICAR REFERENCIAS DE EMPLEOS",
        "ANTERIORES, RECLUTAMIENTO, BOLSA DE TRABAJO, CAPACITACIÓN, EVALUACIÓN Y MEDICIÓN DE HABILIDADES Y",
        "COMPETENCIAS, ASÍ COMO DEFINICIÓN DE ACCIONES DE DESARROLLO, Y EL PAGO DE PRESTACIONES LABORALES."
        },
        new string[] {
        "CLIENTES (PROPONENTES, CONTRATANTES, ASEGURADOS, BENEFICIARIOS Y PROVEEDORES DE RECURSOS). SE",
        "UTILIZARÁN PARA TODOS LOS FINES RELACIONADOS CON EL CUMPLIMIENTO DE NUESTRAS OBLIGACIONES DE",
        "CONFORMIDAD CON LO ESTABLECIDO EN LA LEY SOBRE EL CONTRATO DE SEGURO, PARA EVALUAR SU SOLICITUD DE",
        "SEGURO, SUSCRIPCIÓN, EMISIÓN, TRAMITACIÓN, INVESTIGACIÓN, VERIFICACIÓN, VALIDACIÓN Y CONFIRMACIÓN DE LOS",
        "DATOS PARA LA COTIZACIÓN Y OFRECIMIENTO DE PROGRAMAS DE ASEGURAMIENTO PREVIAMENTE SOLICITADOS; DEL",
        "MISMO MODO, DAR TRÁMITE A SUS RECLAMACIONES DE SINIESTROS DERIVADOS DE DICHOS PROGRAMAS; COBRAR,",
        "ADMINISTRAR, MANTENER O RENOVAR LA PÓLIZA DE SEGURO, PARA ESTUDIOS ESTADÍSTICOS, CUYO TRATAMIENTO PODRÁ",
        "SER DE MANERA INDISTINTA MEDIANTE SUS PROPIOS MEDIOS Y RECURSOS; ASÍ COMO, REMISIÓN DE DICHOS DATOS A",
        "OTRAS INSTITUCIONES DE SEGUROS O ENCARGADOS, CON EL FIN DE QUE ESTÉN EN POSIBILIDAD DE EVALUAR EL RIESGO",
        "DE CUALQUIER PROPUESTA DE ASEGURAMIENTO O BIEN CALIFICAR LA PROCEDENCIA DEL BENEFICIO SOLICITADO EN CASO",
        "DE SINIESTRO, O CUALQUIER DERECHO SOBRE EL SERVICIO CONTRATADO."
        },
        new string[] {
        "IGUALMENTE PODRÁN SER TRATADOS PARA FINALIDADES QUE NO DAN ORIGEN A LA RELACIÓN JURÍDICA ( “FINALIDADES",
        "SECUNDARIAS”), COMO EL OFRECIMIENTO Y PROMOCIÓN DE BIENES, PRODUCTOS Y SERVICIOS, ASÍ COMO LA",
        "PROSPECCIÓN COMERCIAL, EN CUYO CASO SE ENTENDERÁ ACEPTADO DICHO TRATAMIENTO HASTA EN TANTO NO",
        "PROCEDA A INFORMARNOS LO CONTRARIO A TRAVÉS DE LA REVOCACIÓN DEL CONSENTIMIENTO, PUDIENDO MANIFESTARLO",
        "A TRAVÉS DE ARCO_MAPFRE@MAPFRE.COM.MX"
        },
        new string[] {
        "EL PRESENTE AVISO DE MANERA INTEGRAL, ASÍ COMO SUS MODIFICACIONES, ESTARÁN A SU DISPOSICIÓN EN LA",
        "PÁGINA WWW.MAPFRE.COM.MX, A TRAVÉS DE COMUNICADOS COLOCADOS EN NUESTRAS OFICINAS Y SUCURSALES O",
        "INFORMADOS MEDIANTE CUALQUIER MEDIO DE COMUNICACIÓN QUE TENGAMOS CON USTED."
        },
        new string[] {
        "ÚLTIMA ACTUALIZACIÓN: 1º/AGOSTO/2013"
        }
    };
#endregion  

    public static void FillPDF(PdfWriter writer, PdfTemplate template, PdfTemplate under, TemplateValues values, TemplateData data, PolizaStringFactory stringFactory)
    {
        PdfContentByte cb = writer.DirectContent;
        PdfContentByte cbUnder = writer.DirectContentUnder;
        Document document = cb.PdfDocument;
        float cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
        BaseFont pdfFont = values.pdfFont;
        document.NewPage();
        /* aclaraciones*/
        cb.AddTemplate(template, 0, 0);
        if (under != null)
            cbUnder.AddTemplate(under, 0, 0);
        cb.DrawText("ACLARACIONES/OBSERVACIONES O CONDICIONES ESPECIALES: ", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.boxheight;

        if (data.policyData.currency != ApplicationConstants.PESOS_CURRENCY) {

            cb.DrawText("ENDOSO PARA PÓLIZA EN MONEDA EXTRANJERA.", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText + 5.0f;
            for (int parrafo = 0; parrafo < textAclaracionesFP.Length; parrafo++) {
                string[] lineas = textAclaracionesFP[parrafo];
                foreach (string linea in lineas) {
                    cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                    cheight -= TemplateValues.diffText;
                }
                cheight -= 5.0f;
            }
            cheight -= TemplateValues.diffText + 5.0f;
        }
        
        cb.DrawText("ENDOSO PARA EXCLUSIÓN DE RIESGOS CIBERNÉTICOS", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText + 5.0f;
        cb.DrawText("1.- EXCLUSIÓN DE DATOS ELECTRÓNICOS:", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        cb.DrawText("SIN PERJUICIO DE CUALQUIER DISPOSICIÓN CONTRARIA DENTRO DE ESTA PÓLIZA O CUALQUIER ANEXO A LA MISMA, POR LA", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        cb.DrawText("PRESENTE SE ENTIENDE Y SE ACUERDA LO SIGUIENTE:", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText + 5.0f;
        cb.DrawText("A)", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        foreach (string[] parrafo in textPuntoA) {
            foreach (string linea in parrafo) {
                cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 45.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            cheight -= 5.0f;
        }
        if (data.policyData.currency != ApplicationConstants.PESOS_CURRENCY) {
            document.NewPage();
            cb.AddTemplate(template, 0, 0);
            if (under != null)
                cbUnder.AddTemplate(under, 0, 0);
            cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
        }
        /* aclaraciones cont*/
        
        cb.DrawText("B)", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        foreach (string[] parrafo in textPuntoB) {
            foreach (string linea in parrafo) {
                cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 45.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            cheight -= 5.0f;
        }
        cb.DrawText("2.- VALUACIÓN DE LOS MEDIOS DE PROCESAMIENTO ELECTRÓNICO DE DATOS:", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        foreach (string[] parrafo in textAclaracionesSP) {
            foreach (string linea in parrafo) {
                cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            cheight -= 5.0f;
        } 
        
        if (data.policyData.currency == ApplicationConstants.PESOS_CURRENCY) {
            document.NewPage();
            cb.AddTemplate(template, 0, 0);
            if (under != null)
                cbUnder.AddTemplate(under, 0, 0);
            cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
        }
        cheight -= TemplateValues.diffText + 5.0f;
        cb.DrawText("CONDICIONES APLICABLES A ESTA PÓLIZA:", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText + 5.0f;
        
        for (int i = 0; i < textCondicionesAplicables.Length; i++) {
            switch (i) {
                case 1:
                    if (data.policyData.currency == ApplicationConstants.PESOS_CURRENCY) {
                        continue;
                    }
                    break;
                case 2:
                    if (!data.policyData.HMP) {
                        continue;
                    }
                    break;
                case 3:
                    if (!data.policyData.quake) {
                        continue;
                    }
                    break;
                default:
                    break;
            }
            string condicion = textCondicionesAplicables[i];
            cb.DrawText(condicion, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            
        }
        decimal firstRiskAt80Limit = data.policyData.buildingLimit + 
            data.policyData.contentsLimit +
            data.policyData.debriLimit +
            data.policyData.extraLimit;

        if 
        (
            (data.policyData.currency == ApplicationConstants.DOLLARS_CURRENCY && firstRiskAt80Limit > 5000000M) 
            || 
            (data.policyData.currency == ApplicationConstants.PESOS_CURRENCY && firstRiskAt80Limit > (5000000M * getExchangeRate()))
        )
        {
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("RiskAt80Line1"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("RiskAt80Line2"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("RiskAt80Line3"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("RiskAt80Line4"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
            cb.DrawText(stringFactory.GetString("RiskAt80Line5"), pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
            cheight -= TemplateValues.diffText;
        }

        document.NewPage();
        cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
        cb.AddTemplate(template, 0, 0);
        cb.DrawText("ENDOSO A LAS CONDICIONES GENERALES", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        cb.DrawText("DEL CONTRATO DE SEGURO", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText + 5.0f;
        cb.DrawText("ESTIMADO ASEGURADO:", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText;
        foreach (string[] parrafo in textEndosoCondicionesGenerales)
        {
            foreach (string linea in parrafo)
            {
                cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            if (cheight <= 120f)
            {
                document.NewPage();
                cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
                cb.AddTemplate(template, 0, 0);
            }
            else
            {
                cheight -= 5.0f;
            }
        }

        document.NewPage();
        cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
        cb.AddTemplate(template, 0, 0);
        cb.DrawText("AVISO DE PRIVACIDAD SIMPLIFICADO", pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
        cheight -= TemplateValues.diffText + 5.0f;
        foreach (string[] parrafo in textAvisoDePrivacidad)
        {
            foreach (string linea in parrafo)
            {
                cb.DrawText(linea, pdfFont, TemplateValues.textSize, values.left + 30.0f, cheight);
                cheight -= TemplateValues.diffText;
            }
            if (cheight <= 138f)
            {
                document.NewPage();
                cheight = values.top - 50.0f - TemplateValues.boxheight * 4 - 3.0f * 5 - 10.0f;
                cb.AddTemplate(template, 0, 0);
            }
            else
            {
                cheight -= 5.0f;
            }
        }
    }
}
