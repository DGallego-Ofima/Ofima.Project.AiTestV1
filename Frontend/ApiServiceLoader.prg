&& ApiServiceLoader.prg - Utilidad para registrar clases de servicios API

procedure LoadApiServices(tcServicesDir)
	local lcDir
	if vartype(tcServicesDir) = "C" and not empty(tcServicesDir)
		lcDir = addbs(fullpath(tcServicesDir))
	else
		lcDir = addbs(justpath(sys(16, 0)))
		if empty(lcDir)
			lcDir = addbs(curdir())
		endif
	endif

	set procedure to (lcDir + "HttpClient.prg") additive
	set procedure to (lcDir + "BaseApiService.prg") additive
	set procedure to (lcDir + "AuthApiService.prg") additive
	set procedure to (lcDir + "CustomersApiService.prg") additive
	set procedure to (lcDir + "OrdersApiService.prg") additive
endproc
