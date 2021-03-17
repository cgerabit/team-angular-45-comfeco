export function parseApiErrors(response: any): string[]
{
  if (!response) {
    return ['Ha ocurrido un error inesperado'];
  }
  try {
    const result: string[] = [];

    if (response.error) {
      if (typeof response.error === 'string') {
        result.push(response.error);
      } else {
        const errorsMap = response.error.errors;
        if (!errorsMap) {
          return ['Ha ocurrido un error inesperado'];
        }
        const entries = Object.entries(errorsMap);
        entries.forEach((Uarray: any[]) => {
          const campo = Uarray[0];
          Uarray[1].forEach((msg) => {
            result.push(`${campo}:${msg}`);
          });
        });
      }
    } else if (typeof response === 'string') {
      result.push(response);
    }

    return result;
  } catch (exception) {
    return ['Ha ocurrido un error inesperado'];
  }
}

export function generateRandomString(length:number)
{
  const availableCharacters ='ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
  const availableCharactersLength  = availableCharacters.length-1;
  let result ="";
  for(let i =0 ; i<length;i++){
    result+=availableCharacters.charAt(generateRandomInteger(0,availableCharactersLength));
  }

  return result;

}

export function generateRandomInteger(min:number, max:number)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}


export function setOrDeleteFromStorage(key:string,value:string)
{

  if(!key){
    return;
  }
  if(value){
    localStorage.setItem(key,value);
  }
  else{
    localStorage.removeItem(key);
  }
}


