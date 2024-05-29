//function validar() {
//    // Captura el control
//    const txtPrecio = document.getElementById("txtPrecio");
//    const txtCodigo = document.getElementById("txtCodigo");
//    const txtNombre = document.getElementById("txtNombre");
//    const txtDescripcion = document.getElementById("txtDescripcion");

//    let esValido = true; // Bandera para indicar si los campos son válidos

//    // Valida cada campo y agrega las clases
//    if (txtPrecio.value == "") {
//        txtPrecio.classList.add("is-invalid");
//        txtPrecio.classList.remove("is-valid");
//        esValido = false;
//    }
//    else {
//        txtPrecio.classList.remove("is-Invalid");
//        txtPrecio.classList.add("is-valid");
//    }

//    if (txtCodigo.value == "") {
//        txtCodigo.classList.add("is-invalid");
//        txtCodigo.classList.remove("is-valid");
//        esValido = false;
//    }
//    else {
//        txtCodigo.classList.remove("is-invalid");
//        txtCodigo.classList.add("is-valid");
//    }

//    if (txtNombre.value == "") {
//        txtNombre.classList.add("is-invalid");
//        txtNombre.classList.remove("is-valid");
//        esValido = false;
//    }
//    else {
//        txtNombre.classList.remove("is-invalid");
//        txtNombre.classList.add("is-valid");
//    }

//    if (txtDescripcion.value == "") {
//        txtDescripcion.classList.add("is-invalid");
//        txtDescripcion.classList.remove("is-valid");
//        esValido = false;
//    }
//    else {
//        txtDescripcion.classList.remove("is-invalid");
//        txtDescripcion.classList.add("is-valid");
//    }

//    return esValido;
//}

function validacionGeneral() {
    let esValido = true;

    esValido = validarCodigo();
    esValido = validarNombre();
    esValido = validarDescripcion();
    esValido = validarPrecio();

    return esValido;
}

function validarCodigo() {
    // Captura el control
    const txtCodigo = document.getElementById("txtCodigo");
    let esValido = true; // Bandera para indicar si los campos son válidos

    if (txtCodigo.value == "") {
        txtCodigo.classList.add("is-invalid");
        txtCodigo.classList.remove("is-valid");
        esValido = false;
    }
    else {
        txtCodigo.classList.remove("is-invalid");
        txtCodigo.classList.add("is-valid");
    }

    return esValido;
}

function validarNombre() {
    // Captura el control
    const txtNombre = document.getElementById("txtNombre");
    let esValido = true; // Bandera para indicar si los campos son válidos

    if (txtNombre.value == "") {
        txtNombre.classList.add("is-invalid");
        txtNombre.classList.remove("is-valid");
        esValido = false;
    }
    else {
        txtNombre.classList.remove("is-invalid");
        txtNombre.classList.add("is-valid");
    }

    return esValido;
}

function validarDescripcion() {
    // Captura el control
    const txtDescripcion = document.getElementById("txtDescripcion");
    let esValido = true; // Bandera para indicar si los campos son válidos

    if (txtDescripcion.value == "") {
        txtDescripcion.classList.add("is-invalid");
        txtDescripcion.classList.remove("is-valid");
        esValido = false;
    }
    else {
        txtDescripcion.classList.remove("is-invalid");
        txtDescripcion.classList.add("is-valid");
    }

    return esValido;
}

function validarPrecio() {
    // Captura el control
    const txtPrecio = document.getElementById("txtPrecio");
    let esValido = true; // Bandera para indicar si los campos son válidos

    if (txtPrecio.value == "") {
        txtPrecio.classList.add("is-invalid");
        txtPrecio.classList.remove("is-valid");
        esValido = false;
    }
    else {
        txtPrecio.classList.remove("is-Invalid");
        txtPrecio.classList.add("is-valid");
    }

    return esValido;
}