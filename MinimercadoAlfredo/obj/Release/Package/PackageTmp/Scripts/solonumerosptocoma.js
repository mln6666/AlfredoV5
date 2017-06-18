
function solonumerosptocoma() {

    if (event.keyCode == 44) {
        event.preventDefault();
        event.target.value += ".";
    }else{

    if (event.keyCode != 44) {
        if (event.keyCode != 46)
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();

            }
    }
    }
   
    
}



