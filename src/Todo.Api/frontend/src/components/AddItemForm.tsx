import { Alert, Box, Button, Stack, TextField } from "@mui/material";
import { Controller, FormProvider, useForm } from "react-hook-form";
import { TodoApi } from "../api/TodoApi";

type AddItemFormProps = {
    listId?: number,
    disabled?: boolean,
    onAdded: (title: string) => void
}

type AddItemFormData = {
    title: string
}

export default function AddItemForm(props: AddItemFormProps) {
    const form = useForm<AddItemFormData>();

    const onSubmit = async (data: AddItemFormData) => {
        try {
            await TodoApi.addTodoItem(props.listId!, data.title);
            props.onAdded(data.title);
        }
        catch (error) {
            form.setError('root', {
                type: 'api',
                message: error instanceof Error ? error.message : String(error)
            });
        }
    };

    return (
        <Box padding={2} paddingTop={0}>
            <FormProvider {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} autoComplete="off">
                    <Stack direction="row" useFlexGap spacing={2}>
                        <Controller
                            defaultValue=""
                            name="title"
                            control={form.control}
                            render={({ field, fieldState }) => (
                                <TextField
                                    helperText={fieldState.error?.message}
                                    error={Boolean(fieldState.error)}
                                    onChange={field.onChange}
                                    value={field.value}
                                    label="Title"
                                    size="small"
                                    disabled={props.disabled}
                                    fullWidth
                                />
                            )}
                        />
                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                            disabled={props.disabled}
                        >Add</Button>
                    </Stack>
                    {
                        form.formState.errors.root ? (
                            <Alert severity="error">{form.formState.errors.root.message}</Alert>
                        ) : null
                    }
                </form>
            </FormProvider>
        </Box>
    );
}
