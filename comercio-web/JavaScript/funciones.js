function marcarFavorito(elemento) {
    if (elemento.classList.contains("activo")) {
        elemento.classList.remove("activo");
        elemento.src = "Images/Iconos/bookmark.svg";
    }
    else {
        elemento.classList.add("activo");
        elemento.src = "Images/Iconos/bookmark-check.svg";
    }
}