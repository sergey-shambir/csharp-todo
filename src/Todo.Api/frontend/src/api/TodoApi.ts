import axios from "axios"

export type TodoListData = {
    id: number,
    name: string
}

export type TodoListDetailedData = {
    id: number,
    name: string,
    items: TodoItemData[]
}

export type TodoItemData = {
    position: number;
    title: string;
    isComplete: boolean;
}

// TODO: Use either react-query or MobX to track state.
export class TodoApi {
    static async listTodoLists(searchText?: string): Promise<TodoListData[]> {
        const config = searchText ? { params: { search: searchText } } : undefined;
        const response = await axios.get("api/todo-list", config);

        return response.data;
    }

    static async getTodoList(listId: number): Promise<TodoListDetailedData> {
        const response = await axios.get(`api/todo-list/${listId}`);

        return response.data;
    }

    static async createTodoList(name: string): Promise<number> {
        const response = await axios.post("api/todo-list", {
            name: name
        });

        return response.data;
    }

    static async addTodoItem(listId: number, title: string): Promise<number> {
        const response = await axios.post(`api/todo-list/${listId}`, {
            title: title
        });

        return response.data;
    }

    static async renameTodoItem(listId: number, position: number, newTitle: string): Promise<void> {
        await axios.patch(`api/todo-list/${listId}/${position}`, {
            title: newTitle
        });
    }

    static async changeTodoItemStatus(listId: number, position: number, newIsCompleted: boolean): Promise<void> {
        await axios.patch(`api/todo-list/${listId}/${position}`, {
            isCompleted: newIsCompleted
        });
    }

    static async moveTodoItem(listId: number, position: number, newPosition: number): Promise<void> {
        await axios.patch(`api/todo-list/${listId}/${position}`, {
            position: newPosition
        });
    }

    static async deleteTodoItem(listId: number, position: number): Promise<void> {
        await axios.delete(`api/todo-list/${listId}/${position}`);
    }

    static async deleteList(listId: number): Promise<void> {
        await axios.delete(`api/todo-list/${listId}`);
    }
}
