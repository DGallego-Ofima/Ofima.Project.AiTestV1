&& OrdersApiService.prg - Consumo de endpoints de pedidos

DEFINE CLASS OrdersApiService AS BaseApiService

    PROCEDURE List(tnPage, tnPageSize, tcStatus, tcFromDate, tcToDate)
        LOCAL lcEndpoint, lcParams
        lcEndpoint = "/api/v1/orders"
        lcParams = ""
        lcParams = This.AddQueryParam(lcParams, "page", tnPage)
        lcParams = This.AddQueryParam(lcParams, "pageSize", tnPageSize)
        lcParams = This.AddQueryParam(lcParams, "status", tcStatus)
        lcParams = This.AddQueryParam(lcParams, "from", tcFromDate)
        lcParams = This.AddQueryParam(lcParams, "to", tcToDate)
        IF NOT EMPTY(lcParams)
            lcEndpoint = lcEndpoint + "?" + lcParams
        ENDIF
        RETURN This.Get(lcEndpoint)
    ENDPROC

    PROCEDURE GetById(tnOrderId)
        RETURN This.Get("/api/v1/orders/" + This.FormatId(tnOrderId))
    ENDPROC

    PROCEDURE Create(tcOrderJson)
        RETURN This.Post("/api/v1/orders", IIF(VARTYPE(tcOrderJson) = "C", tcOrderJson, "{}"))
    ENDPROC

    PROCEDURE Update(tnOrderId, tcOrderJson)
        RETURN This.Put("/api/v1/orders/" + This.FormatId(tnOrderId), ;
            IIF(VARTYPE(tcOrderJson) = "C", tcOrderJson, "{}"))
    ENDPROC

    PROCEDURE Confirm(tnOrderId)
        RETURN This.Post("/api/v1/orders/" + This.FormatId(tnOrderId) + "/confirm", "{}")
    ENDPROC

    PROCEDURE Cancel(tnOrderId)
        RETURN This.Post("/api/v1/orders/" + This.FormatId(tnOrderId) + "/cancel", "{}")
    ENDPROC

    PROCEDURE AddQueryParam(tcParams, tcKey, tvValue)
        LOCAL lcParams, lcValue
        lcParams = IIF(VARTYPE(tcParams) = "C", tcParams, "")
        lcValue = ""
        DO CASE
        CASE VARTYPE(tvValue) = "N" AND tvValue > 0
            lcValue = LTRIM(TRANSFORM(tvValue))
        CASE VARTYPE(tvValue) = "C"
            lcValue = ALLTRIM(tvValue)
        ENDCASE
        IF EMPTY(lcValue)
            RETURN lcParams
        ENDIF
        IF NOT EMPTY(lcParams)
            lcParams = lcParams + "&"
        ENDIF
        lcParams = lcParams + tcKey + "=" + This.UrlEncode(lcValue)
        RETURN lcParams
    ENDPROC

    PROCEDURE UrlEncode(tcValue)
        LOCAL lcValue
        lcValue = IIF(VARTYPE(tcValue) = "C", tcValue, "")
        lcValue = STRTRAN(lcValue, "%", "%25")
        lcValue = STRTRAN(lcValue, " ", "%20")
        lcValue = STRTRAN(lcValue, "+", "%2B")
        lcValue = STRTRAN(lcValue, ":", "%3A")
        lcValue = STRTRAN(lcValue, "/", "%2F")
        lcValue = STRTRAN(lcValue, ",", "%2C")
        RETURN lcValue
    ENDPROC

ENDDEFINE
