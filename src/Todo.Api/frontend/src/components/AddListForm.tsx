import { Controller, FormProvider, useForm } from "react-hook-form";
import { TodoApi } from "../api/TodoApi";
import { Alert, Box, Button, Stack, TextField } from "@mui/material";


type AddListFormProps = {
    disabled?: boolean,
    onAdded: (listId: number, name: string) => void
}

type AddListFormData = {
    name: string
}

export default function AddListForm(props: AddListFormProps) {
    const form = useForm<AddListFormData>();

    const onSubmit = async (data: AddListFormData) => {
        try {
            const listId: number = await TodoApi.createTodoList(data.name);
            props.onAdded(listId, data.name);
        }
        catch (error) {
            form.setError('root', {
                type: 'api',
                message: error instanceof Error ? error.message : String(error)
            });
        }
    }

    return (
        <Box padding={2} paddingTop={0}>
            <FormProvider {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} autoComplete="off">
                    <Stack direction="row" useFlexGap spacing={2}>
                        <Controller
                            defaultValue=""
                            name="name"
                            control={form.control}
                            render={({ field, fieldState }) => (
                                <TextField
                                    helperText={fieldState.error?.message}
                                    error={Boolean(fieldState.error)}
                                    onChange={field.onChange}
                                    value={field.value}
                                    label="Name"
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
