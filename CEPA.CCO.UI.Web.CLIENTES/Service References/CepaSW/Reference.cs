﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CEPA.CCO.UI.Web.CLIENTES.CepaSW {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://aduana.gob.sv/", ConfigurationName="CepaSW.WSManifiestoCEPA")]
    public interface WSManifiestoCEPA {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/getContenedorDataRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/getContenedorDataResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataResponse getContenedorData(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/getDocumentoInfoDocumentoRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/getDocumentoInfoDocumentoResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoResponse getDocumentoInfoDocumento(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/getCepaDataRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/getCepaDataResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataResponse getCepaData(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/updateCepaDataRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/updateCepaDataResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataResponse updateCepaData(CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/consumeWsCepaRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/consumeWsCepaResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaResponse consumeWsCepa(CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/insertContenedorDANRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/insertContenedorDANResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANResponse insertContenedorDAN(CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequest request);
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento xmlData del espacio de nombres  no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://aduana.gob.sv/WSManifiestoCEPA/validaContenedorDeclaRequest", ReplyAction="http://aduana.gob.sv/WSManifiestoCEPA/validaContenedorDeclaResponse")]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaResponse validaContenedorDecla(CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getContenedorDataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getContenedorData", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequestBody Body;
        
        public getContenedorDataRequest() {
        }
        
        public getContenedorDataRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class getContenedorDataRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public getContenedorDataRequestBody() {
        }
        
        public getContenedorDataRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getContenedorDataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getContenedorDataResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataResponseBody Body;
        
        public getContenedorDataResponse() {
        }
        
        public getContenedorDataResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class getContenedorDataResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public getContenedorDataResponseBody() {
        }
        
        public getContenedorDataResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getDocumentoInfoDocumentoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getDocumentoInfoDocumento", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequestBody Body;
        
        public getDocumentoInfoDocumentoRequest() {
        }
        
        public getDocumentoInfoDocumentoRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class getDocumentoInfoDocumentoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public getDocumentoInfoDocumentoRequestBody() {
        }
        
        public getDocumentoInfoDocumentoRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getDocumentoInfoDocumentoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getDocumentoInfoDocumentoResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoResponseBody Body;
        
        public getDocumentoInfoDocumentoResponse() {
        }
        
        public getDocumentoInfoDocumentoResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class getDocumentoInfoDocumentoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public getDocumentoInfoDocumentoResponseBody() {
        }
        
        public getDocumentoInfoDocumentoResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getCepaDataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getCepaData", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequestBody Body;
        
        public getCepaDataRequest() {
        }
        
        public getCepaDataRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class getCepaDataRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public getCepaDataRequestBody() {
        }
        
        public getCepaDataRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getCepaDataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getCepaDataResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataResponseBody Body;
        
        public getCepaDataResponse() {
        }
        
        public getCepaDataResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class getCepaDataResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public getCepaDataResponseBody() {
        }
        
        public getCepaDataResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class updateCepaDataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="updateCepaData", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequestBody Body;
        
        public updateCepaDataRequest() {
        }
        
        public updateCepaDataRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class updateCepaDataRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public updateCepaDataRequestBody() {
        }
        
        public updateCepaDataRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class updateCepaDataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="updateCepaDataResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataResponseBody Body;
        
        public updateCepaDataResponse() {
        }
        
        public updateCepaDataResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class updateCepaDataResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public updateCepaDataResponseBody() {
        }
        
        public updateCepaDataResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class consumeWsCepaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="consumeWsCepa", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequestBody Body;
        
        public consumeWsCepaRequest() {
        }
        
        public consumeWsCepaRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class consumeWsCepaRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public consumeWsCepaRequestBody() {
        }
        
        public consumeWsCepaRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class consumeWsCepaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="consumeWsCepaResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaResponseBody Body;
        
        public consumeWsCepaResponse() {
        }
        
        public consumeWsCepaResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class consumeWsCepaResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public consumeWsCepaResponseBody() {
        }
        
        public consumeWsCepaResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class insertContenedorDANRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="insertContenedorDAN", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequestBody Body;
        
        public insertContenedorDANRequest() {
        }
        
        public insertContenedorDANRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class insertContenedorDANRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public insertContenedorDANRequestBody() {
        }
        
        public insertContenedorDANRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class insertContenedorDANResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="insertContenedorDANResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANResponseBody Body;
        
        public insertContenedorDANResponse() {
        }
        
        public insertContenedorDANResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class insertContenedorDANResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public insertContenedorDANResponseBody() {
        }
        
        public insertContenedorDANResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class validaContenedorDeclaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="validaContenedorDecla", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequestBody Body;
        
        public validaContenedorDeclaRequest() {
        }
        
        public validaContenedorDeclaRequest(CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class validaContenedorDeclaRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string xmlData;
        
        public validaContenedorDeclaRequestBody() {
        }
        
        public validaContenedorDeclaRequestBody(string xmlData) {
            this.xmlData = xmlData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class validaContenedorDeclaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="validaContenedorDeclaResponse", Namespace="http://aduana.gob.sv/", Order=0)]
        public CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaResponseBody Body;
        
        public validaContenedorDeclaResponse() {
        }
        
        public validaContenedorDeclaResponse(CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="")]
    public partial class validaContenedorDeclaResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string @return;
        
        public validaContenedorDeclaResponseBody() {
        }
        
        public validaContenedorDeclaResponseBody(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WSManifiestoCEPAChannel : CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSManifiestoCEPAClient : System.ServiceModel.ClientBase<CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA>, CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA {
        
        public WSManifiestoCEPAClient() {
        }
        
        public WSManifiestoCEPAClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WSManifiestoCEPAClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSManifiestoCEPAClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSManifiestoCEPAClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.getContenedorData(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequest request) {
            return base.Channel.getContenedorData(request);
        }
        
        public string getContenedorData(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.getContenedorDataResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).getContenedorData(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.getDocumentoInfoDocumento(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequest request) {
            return base.Channel.getDocumentoInfoDocumento(request);
        }
        
        public string getDocumentoInfoDocumento(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.getDocumentoInfoDocumentoResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).getDocumentoInfoDocumento(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.getCepaData(CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequest request) {
            return base.Channel.getCepaData(request);
        }
        
        public string getCepaData(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.getCepaDataResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).getCepaData(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.updateCepaData(CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequest request) {
            return base.Channel.updateCepaData(request);
        }
        
        public string updateCepaData(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.updateCepaDataResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).updateCepaData(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.consumeWsCepa(CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequest request) {
            return base.Channel.consumeWsCepa(request);
        }
        
        public string consumeWsCepa(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.consumeWsCepaResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).consumeWsCepa(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.insertContenedorDAN(CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequest request) {
            return base.Channel.insertContenedorDAN(request);
        }
        
        public string insertContenedorDAN(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.insertContenedorDANResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).insertContenedorDAN(inValue);
            return retVal.Body.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaResponse CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA.validaContenedorDecla(CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequest request) {
            return base.Channel.validaContenedorDecla(request);
        }
        
        public string validaContenedorDecla(string xmlData) {
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequest inValue = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequest();
            inValue.Body = new CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaRequestBody();
            inValue.Body.xmlData = xmlData;
            CEPA.CCO.UI.Web.CLIENTES.CepaSW.validaContenedorDeclaResponse retVal = ((CEPA.CCO.UI.Web.CLIENTES.CepaSW.WSManifiestoCEPA)(this)).validaContenedorDecla(inValue);
            return retVal.Body.@return;
        }
    }
}
