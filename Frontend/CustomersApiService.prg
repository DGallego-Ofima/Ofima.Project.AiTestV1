&& CustomersApiService.prg - Consumo de endpoints de clientes

DEFINE CLASS CustomersApiService AS BaseApiService

    PROCEDURE List(tlActiveOnly)
        LOCAL lcEndpoint
        lcEndpoint = "/api/v1/customers"
        IF VARTYPE(tlActiveOnly) = "L" AND tlActiveOnly
            lcEndpoint = lcEndpoint + "?active=true"
        ENDIF
        RETURN This.Get(lcEndpoint)
    ENDPROC

    PROCEDURE GetById(tnCustomerId)
        RETURN This.Get("/api/v1/customers/" + This.FormatId(tnCustomerId))
    ENDPROC

    PROCEDURE Create(tcCustomerJson)
        RETURN This.Post("/api/v1/customers", IIF(VARTYPE(tcCustomerJson) = "C", tcCustomerJson, "{}"))
    ENDPROC

    PROCEDURE Update(tnCustomerId, tcCustomerJson)
        RETURN This.Put("/api/v1/customers/" + This.FormatId(tnCustomerId), ;
            IIF(VARTYPE(tcCustomerJson) = "C", tcCustomerJson, "{}"))
    ENDPROC

    PROCEDURE Delete(tnCustomerId)
        RETURN This.Delete("/api/v1/customers/" + This.FormatId(tnCustomerId))
    ENDPROC

ENDDEFINE
