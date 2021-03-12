export interface User{
    username: string;
    token: string;
}

let data: number | string = 42;
data = "10";

interface car {
    color: string,
    brand?: number | string,
    techv?: number
}

const car1: car = {
    color: 'blue',
    brand: 'bmw'
}

const car2: car = {
    color: 'red',
    techv: 3
}

const car3: car = {
    color: 'green',
    brand: 45,
    techv: 3
}

const mult = (x: number | string, y: number, z: number) => {
    return 'x:'+ y*z;
}