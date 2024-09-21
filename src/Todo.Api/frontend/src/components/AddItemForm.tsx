import { Box, Button, Stack, TextField } from "@mui/material";
import { Controller, FormProvider, useForm } from "react-hook-form";

type AddTodoItemData = {
    title: string
}

export default function AddItemForm() {
    const form = useForm<AddTodoItemData>();

    const onSubmit = (data: AddTodoItemData) => console.log(data);

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
                                    fullWidth
                                />
                            )}
                        />
                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                        >Add</Button>
                    </Stack>
                </form>
            </FormProvider>
        </Box>
    );
}
