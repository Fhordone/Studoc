$(document).ready(function () {
    var pasosContainer = $("#pasosContainer");

    $("#btnAgregarPaso").click(function () {
        var pasoIndex = pasosContainer.find(".panel-default").length;

        var nuevoPasoHtml = `
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Paso <span id="pasoIndex">${pasoIndex + 1}</span></h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <label for="Pasos[${pasoIndex}].Titulo" class="control-label">Título</label>
                            <input type="text" name="Pasos[${pasoIndex}].Titulo" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="Pasos[${pasoIndex}].Contenido" class="control-label">Contenido</label>
                            <textarea name="Pasos[${pasoIndex}].Contenido" class="form-control" rows="4"></textarea>
                        </div>
                        <div class="form-group">
                            <label for="Pasos[${pasoIndex}].ImagenFile" class="control-label">Imagen</label>
                            <input type="file" name="Pasos[${pasoIndex}].ImagenFile" class="form-control" />
                        </div>
                    </div>
                </div>`;

        // Agrega el nuevo paso al final del contenedor
        pasosContainer.append(nuevoPasoHtml);
    });
});