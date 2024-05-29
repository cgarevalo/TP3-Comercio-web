// Función para previsualizar la imagen seleccionada en el control FileUpload antes de cargarla en el servidor
function preVisualizarImagen() {
    console.log("preVisualizarImagen function called");

    // Obtiene el control fileUpload y el control Image por sus Id
    const fileUpload = document.getElementById("<%= fuImagenArt.ClientID %>");
    const imgPerfil = document.getElementById("<%= imgImagenArt.ClientID %>");
    //const fileUpload = document.getElementById("fuImagenArt");
    //const imgPerfil = document.getElementById("imgImagenArt");

    // Verifica si se ha seleccionado un archivo
    if (fileUpload.files && fileUpload.files[0]) {
        console.log("File selected: ", fileUpload.files[0]);

        // Crea un nuevo fileReader para leer el contenido del archivo
        let reader = new FileReader();

        // Define la función que se ejecuta cuando el archivo se ha leído completamente
        reader.onload = function (e) {
            console.log("FileReader onload event: ", e.target.result);

            // Asigna el contenido leído (URL de la imagen) al control Image para previsualización
            imgPerfil.src = e.target.result;
        };

        // Lee el contenido del archivo como una URL de datos
        reader.readAsDataURL(fileUpload.files[0]);
    }
    else {
        console.log("No file selected");
    }
}