import { Respuesta } from "./Respuesta";
import { User } from "./User";

export class Sesion extends Respuesta {
    public Token: string;
    public Persona: User;
}
