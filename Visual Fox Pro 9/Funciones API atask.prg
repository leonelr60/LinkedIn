
    PROCEDURE ATask(cTitle as Character,  cMatriz  as Object, lOcultos as Boolean , iTot as Boolean ) as Integer 
	SET UDFPARMS TO VALUE
	*** Constantes
    #define GW_HWNDFIRST        0
    #define GW_HWNDNEXT         2
    #define GW_OWNER            4
    #define GW_CHILD            5
    *** Declaraciones de la funciones del API
    DECLARE ;
      Integer GetWindow ;
      IN WIN32API ;
      Integer  nHwnd, ;
      Integer  nCmd
    DECLARE ;
      Integer GetWindowText ;
      IN WIN32API ;
      Integer  nHwnd, ;
      String  @cString, ;
      Integer  nMaxCount
    DECLARE ;
      Integer GetWindowTextLength ;
      IN WIN32API ;
      Integer  nWnd
    DECLARE ;
      Integer IsWindowVisible ;
      IN WIN32API ;
      Integer  nWnd
    DECLARE ;
      Integer GetDesktopWindow ;
      IN WIN32API

    *** Declaración de variables
    PRIVATE nFoxHwnd, nCont, nCurrWnd
    PRIVATE nLength, cTmp
    PRIVATE sReturn
    RELEASE MEMORY cMatriz
    *PUBLIC cMatriz
	STORE "" TO cTmp
    *** Obtención del handle del DeskTop
    nHwnd = GetDesktopWindow()
    nInitHwnd = GetWindow( nHwnd, GW_CHILD )

    *** Esta será la primera ventana
    nCurrWnd = GetWindow( nInitHwnd, ;
                          GW_HWNDFIRST )
    *** Inicializar contador
    nCont = 0

    *** Recorrer todas las ventanas
    DO WHILE nCurrWnd # 0

      *** Comprobar si no tiene padre
      IF GetWindow(nCurrWnd, GW_OWNER) = 0

        *** Si debemos las ventanas ocultas
        IF IsWindowVisible( nCurrWnd ) = 1 ;
           OR lOcultos 

          *** Tamaño del título
          nLength=GetWindowTextLength(nCurrWnd)
          IF nLength > 0

            nCont = nCont + 1

            *** Obtener el título
            cTmp=REPLICATE( CHR(0), nLength+1 )
            =GetWindowText( nCurrWnd, ;
                            @cTmp, ;
                            nLength + 1 )
            *** Insertar un nuevo elemento
            DIMENSION cMatriz[nCont,2]
            cMatriz[nCont,1] = SUBSTR( cTmp, 1, nLength )
            cMatriz[nCont,2] = nCurrWnd
            
            *STORE SUBSTR( cTmp, 1, nLength ) TO sVentana
            *STORE cTitle TO sFactor

	          *IF(sVentana == sFactor)

		        *RELEASE MEMORY cMatriz
		      *  sReturn = sFactor 
		        *EXIT 
		      *ENDIF
          ENDIF
        ENDIF
      ENDIF
      *** Obtener la siguiente ventana
      nCurrWnd = GetWindow( nCurrWnd, ;
                            GW_HWNDNEXT )
    ENDDO && (nCurrWnd # 0)
    
    
    RELEASE MEMORY cMatriz

    	RETURN nCont
    
    