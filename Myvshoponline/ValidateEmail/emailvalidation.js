    function ValidateEmail(email) {
        var regex =/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        if (email.match(regex)) {
            return true;
        }
        else {
           // alert("Invalid Email")
        }
    }
