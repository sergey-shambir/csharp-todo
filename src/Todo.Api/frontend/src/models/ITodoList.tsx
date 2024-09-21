import ITodoItem from "./ITodoItem";

export default interface ITodoList
{
    id: number,
    name: string,
    items: ITodoItem[]
}