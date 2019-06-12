var popUp; 

function UpdateTexto(idname,valor)
{
var sData = dialogArguments;
sData.document.getElementById(idname).value = valor;
}


function OpenCatalago(idname, postBack, Ancho, Alto, PaginaWeb)
{
x = (screen.width - Ancho) / 2;
y = (screen.height - Alto) / 2;
popUp = window.open(PaginaWeb + '?formname=' + document.forms[0].name + 
		'&id=' + idname + '&selected=' + encodeURIComponent(document.forms[0].elements[idname].value) + '&postBack=' + postBack, 
		'popupcal', 
		'width=' + Ancho + ',height=' + Alto + ',left=' + x + ',top=' + y + '');
}


function OpenCata(PaginaWeb, Ancho, Alto) {
    x = (screen.width - Ancho) / 2;
    y = (screen.height - Alto) / 2;
    popUp = window.open(PaginaWeb, 'popup', 'width=' + Ancho + ',height=' + Alto + ',dependent=YES,resizable=NO,status=NO,scrollbars=NO');
}


function VentanaDialogoModal(url,Arg,Ancho, Alto)
{
x = (screen.width - Ancho) / 2;
y = (screen.height - Alto) / 2;
var resultado;
resultado = window.showModalDialog(url, Arg, 'dialogHeight: ' + Alto + 'px; dialogWidth: ' + Ancho + 'px; dialogTop: ' + y + 'px; dialogLeft: ' + x + 'px; edge: Raised; center: Yes; help: No; resizable: No; status: No;');
//resultado = window.showModalDialog(url, Arg, 'dialogHeight: ' + Alto + 'px; dialogWidth: ' + Ancho + 'px; dialogTop: ' + y + 'px; dialogLeft: ' + x + 'px;  edge: Raised; center: yes; help: no; resizable: no; status: no;');
//alert(resultado);

}


function VentanaDialogoNoModal(url,Arg,Ancho, Alto, idname)
{
x = (screen.width - Ancho) / 2;
y = (screen.height - Alto) / 2;
var resultado;
resultado = window.showModelessDialog(url,Arg, 'dialogHeight: '+ Alto +'px; dialogWidth: '+ Alto+'px; dialogTop: '+ y +'px; dialogLeft: '+ x +'px; edge: Raised; center: Yes; help: No; resizable: No; status: No;');
}


function SetDatos(formName, idname, newData, postBack)
{
	eval('var theform = document.' + formName + ';');
	popUp.close();
	theform.elements[idname].value = newData;
	if (postBack)
		__doPostBack(id,'');
}		

