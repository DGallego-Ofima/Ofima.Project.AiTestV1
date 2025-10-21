&& BaseApiService.prg - Clase base para servicios de consumo de API REST

DEFINE CLASS BaseApiService AS Custom
    oHttp = .NULL.
    cBaseUrl = ""
    cToken = ""
    cLastError = ""

    PROCEDURE Init(toHttpClient)
        IF VARTYPE(toHttpClient) = "O"
            This.oHttp = toHttpClient
            This.cBaseUrl = IIF(PEMSTATUS(toHttpClient, "cBaseUrl", 5), toHttpClient.cBaseUrl, "")
        ELSE
            This.EnsureClient()
        ENDIF
    ENDPROC

    PROCEDURE EnsureClient
        IF VARTYPE(This.oHttp) <> "O"
            This.oHttp = CREATEOBJECT("HttpClient", This.cBaseUrl)
        ENDIF
    ENDPROC

    PROCEDURE SetBaseUrl(tcBaseUrl)
        This.cBaseUrl = IIF(VARTYPE(tcBaseUrl) = "C", tcBaseUrl, "")
        This.EnsureClient()
        IF VARTYPE(This.oHttp) = "O"
            This.oHttp.SetBaseUrl(This.cBaseUrl)
        ENDIF
    ENDPROC

    PROCEDURE SetToken(tcToken)
        This.cToken = IIF(VARTYPE(tcToken) = "C", tcToken, "")
    ENDPROC

    PROCEDURE ClearToken
        This.cToken = ""
    ENDPROC

    PROCEDURE GetLastError
        RETURN This.cLastError
    ENDPROC

    PROCEDURE Send(tcMethod, tcEndpoint, tcBody)
        LOCAL lcBody, loResponse
        lcBody = IIF(VARTYPE(tcBody) = "C", tcBody, "")
        This.EnsureClient()
        loResponse = This.oHttp.Send(tcMethod, tcEndpoint, lcBody, This.BuildHeaders())
        This.cLastError = IIF(loResponse.Success, "", This.oHttp.cLastError)
        RETURN loResponse
    ENDPROC

    PROCEDURE Get(tcEndpoint)
        RETURN This.Send("GET", tcEndpoint, "")
    ENDPROC

    PROCEDURE Post(tcEndpoint, tcBody)
        RETURN This.Send("POST", tcEndpoint, tcBody)
    ENDPROC

    PROCEDURE Put(tcEndpoint, tcBody)
        RETURN This.Send("PUT", tcEndpoint, tcBody)
    ENDPROC

    PROCEDURE Patch(tcEndpoint, tcBody)
        RETURN This.Send("PATCH", tcEndpoint, tcBody)
    ENDPROC

    PROCEDURE Delete(tcEndpoint)
        RETURN This.Send("DELETE", tcEndpoint, "")
    ENDPROC

    PROCEDURE BuildHeaders
        LOCAL loHeaders
        loHeaders = CREATEOBJECT("Empty")
        ADDPROPERTY(loHeaders, "Accept", "application/json")
        IF NOT EMPTY(This.cToken)
            ADDPROPERTY(loHeaders, "Authorization", "Bearer " + This.cToken)
        ENDIF
        RETURN loHeaders
    ENDPROC

    PROCEDURE EscapeJson(tcValue)
        LOCAL lcValue
        lcValue = IIF(VARTYPE(tcValue) = "C", tcValue, "")
        lcValue = STRTRAN(lcValue, CHR(92), CHR(92) + CHR(92))
        lcValue = STRTRAN(lcValue, '"', CHR(92) + '"')
        lcValue = STRTRAN(lcValue, CHR(13), CHR(92) + "r")
        lcValue = STRTRAN(lcValue, CHR(10), CHR(92) + "n")
        RETURN lcValue
    ENDPROC

    PROCEDURE FormatId(tvValue)
        DO CASE
        CASE VARTYPE(tvValue) = "N"
            RETURN LTRIM(TRANSFORM(tvValue))
        CASE VARTYPE(tvValue) = "C"
            RETURN ALLTRIM(tvValue)
        OTHERWISE
            RETURN "0"
        ENDCASE
    ENDPROC
ENDDEFINE
