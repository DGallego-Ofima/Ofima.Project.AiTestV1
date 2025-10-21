&& HttpClient.prg - Cliente HTTP generico para Visual FoxPro
&& Usa WinHTTP para realizar peticiones REST contra la API .NET

#define ERR_TIMEOUT 408
#define ERR_GENERAL 500

define class HttpClient as custom
	cBaseUrl = ""
	nTimeout = 30000 && milisegundos
	cUserAgent = "OfimaVFPClient/1.0"
	cLastError = ""

	procedure init(tcBaseUrl)
		if vartype(tcBaseUrl) = "C"
			this.cBaseUrl = tcBaseUrl
		endif
	endproc

	procedure SetBaseUrl(tcBaseUrl)
		this.cBaseUrl = iif(empty(tcBaseUrl), "", tcBaseUrl)
	endproc

	procedure SetTimeout(tnTimeout)
		if vartype(tnTimeout) = "N" and tnTimeout > 0
			this.nTimeout = tnTimeout
		endif
	endproc

	procedure send(tcMethod, tcEndpoint, tcBody, toHeaders)
		local loHttp, lcUrl, lcMethod, lcBody, lcResponse
		local llSuccess, lnStatus, loHeaders

		lcMethod = upper(iif(empty(tcMethod), "GET", tcMethod))
		lcUrl = this.BuildUrl(tcEndpoint)
		lcBody = iif(vartype(tcBody) = "C", tcBody, "")
		loHeaders = iif(vartype(toHeaders) = "O", toHeaders, .null.)
		this.cLastError = ""
		llSuccess = .f.
		lnStatus = ERR_GENERAL
		lcResponse = ""

		try
			loHttp = createobject("WinHttp.WinHttpRequest.5.1")
			loHttp.open(lcMethod, lcUrl, .f.)
			loHttp.SetTimeouts(this.nTimeout, this.nTimeout, this.nTimeout, this.nTimeout)
			loHttp.SetRequestHeader("User-Agent", this.cUserAgent)
			loHttp.SetRequestHeader("Content-Type", "application/json; charset=utf-8")

			if vartype(loHeaders) = "O"
				this.ApplyHeaders(loHttp, loHeaders)
			endif

			if inlist(lcMethod, "POST", "PUT", "PATCH")
				loHttp.send(lcBody)
			else
				loHttp.send()
			endif

			loHttp.WaitForResponse(this.nTimeout / 1000)
			lnStatus = loHttp.status
			lcResponse = loHttp.ResponseText
			llSuccess = (lnStatus >= 200 and lnStatus < 300)

			if not llSuccess
				this.cLastError = "HTTP " + transform(lnStatus)
			endif
		catch to loEx
			this.cLastError = transform(loEx.message)
			llSuccess = .f.
			lnStatus = ERR_GENERAL
			lcResponse = ""
		finally
			loHttp = .null.
		EndTry
		
		return this.BuildResponse(llSuccess, lnStatus, lcResponse)
	endproc

	procedure BuildUrl(tcEndpoint)
		local lcEndpoint, lcBase
		lcEndpoint = iif(left(tcEndpoint, 1) = "/", substr(tcEndpoint, 2), tcEndpoint)
		lcBase = alltrim(this.cBaseUrl)
		if empty(lcBase)
			return tcEndpoint
		endif
		if right(lcBase, 1) = "\\"
			lcBase = substr(lcBase, 1, len(lcBase) - 1)
		endif
		if right(lcBase, 1) <> "/"
			lcBase = lcBase + "/"
		endif
		return lcBase + lcEndpoint
	endproc

	procedure ApplyHeaders(toHttp, toHeaders)
		local laMembers[1], lnCount, lnI
		lnCount = amembers(laMembers, toHeaders)
		for lnI = 1 to lnCount
			toHttp.SetRequestHeader(laMembers[lnI], evaluate("toHeaders." + laMembers[lnI]))
		endfor
	endproc

	procedure BuildResponse(tlSuccess, tnStatus, tcBody)
		local loResp
		loResp = createobject("Empty")
		addproperty(loResp, "Success", tlSuccess)
		addproperty(loResp, "Status", tnStatus)
		addproperty(loResp, "Body", iif(vartype(tcBody) = "C", tcBody, ""))
		return loResp
	endproc
enddefine
