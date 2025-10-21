&& AuthApiService.prg - Consumo del endpoint de autenticacion
&& Requiere BaseApiService.prg y HttpClient.prg cargados previamente con SET PROCEDURE.

DEFINE CLASS AuthApiService AS BaseApiService

    PROCEDURE Login(tcUsername, tcPassword)
        LOCAL lcBody
        lcBody = "{" + ;
            '"username":"' + This.EscapeJson(tcUsername) + '",' + ;
            '"password":"' + This.EscapeJson(tcPassword) + '"' + ;
            "}"
        RETURN This.Post("/api/v1/auth/login", lcBody)
    ENDPROC

    PROCEDURE ValidateToken(tcToken)
        LOCAL lcBody
        lcBody = "{" + ;
            '"token":"' + This.EscapeJson(tcToken) + '"' + ;
            "}"
        RETURN This.Post("/api/v1/auth/validate", lcBody)
    ENDPROC

ENDDEFINE
