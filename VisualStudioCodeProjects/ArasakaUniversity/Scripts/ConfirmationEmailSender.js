const email = localStorage.getItem('userEmail');

export async function SendConfirmationEmailAsync(email){
    const url = new URL("http://localhost:5238/authentication/send-confirm-email");

    console.log(email);


    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify( email ) 
    };

    await fetch(url.href, requestOptions)
    .then(response => response)
    .catch(error => console.log(error));
     
}
