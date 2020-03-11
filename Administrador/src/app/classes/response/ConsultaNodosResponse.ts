import { Nodo } from "../Nodo";
import { Respuesta } from "../Respuesta";

export class ConsultaNodosResponse extends Respuesta {
    public nodos: Nodo[];
}