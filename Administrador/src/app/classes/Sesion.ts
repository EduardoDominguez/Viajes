import { Respuesta } from "./Respuesta";
import { User } from "./User";

export class Sesion extends Respuesta {
    public token: string;
    public user: User;
}