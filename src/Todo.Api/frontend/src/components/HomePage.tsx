import { Alert, Box, Container, List, Paper, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { TodoApi, TodoListData } from "../api/TodoApi";
import TodoListView from "./TodoListView";
import AddListForm from "./AddListForm";

type HomePageData = {
    isLoading: boolean,
    lists?: TodoListData[],
    error?: string
}

export default function HomePage() {
    const [data, setData] = useState<HomePageData>({
        isLoading: true
    });

    useEffect(() => {
        TodoApi.listTodoLists().then(
            lists => setData({
                isLoading: false,
                lists: lists
            }),
            error => setData({
                isLoading: false,
                error: error.message
            })
        );
    }, [])


    return (
        <Container maxWidth="sm">
            <Typography variant="h4" textAlign="center" gutterBottom>To-Do</Typography>
            <Paper elevation={3}>
                {
                    (data.lists && data.lists.length > 0) ? (
                        <List dense>
                            {data.lists.map((list: TodoListData) => (
                                <TodoListView list={list}></TodoListView>
                            ))}
                        </List>
                    ) : (
                        <Box padding={2}>
                            <Typography>No todo lists yet...</Typography>
                        </Box>
                    )
                }
                <AddListForm
                    disabled={data.isLoading}
                    onAdded={(listId: number, name: string) => {
                        setData({
                            ...data,
                            lists: [
                                ...data.lists!,
                                { id: listId, name: name }
                            ]
                        })
                    }}
                ></AddListForm>
                {data.error ? (
                    <Alert severity="error">{data.error}</Alert>
                ) : null}
            </Paper>
        </Container>
    )
}