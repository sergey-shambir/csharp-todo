Функциональность: TODO
    Как пользователь
    Я хочу вести свои списки дел
    Чтобы систематизировать ежедневную рутину

@positive
Сценарий: редактирование одного списка
    Пусть я создал список "Домашние дела"
    И добавил задачу "Убрать комнату"
    И добавил задачу "Съесть борщ"
    И добавил задачу "Приготовить борщ"
    И добавил задачу "Покормить хомяка"
    Когда я открыл созданный список
    Тогда вижу задачи: "Убрать комнату, Съесть борщ, Приготовить борщ, Покормить хомяка"

    Пусть я переместил задачу №3 на позицию №2
    Когда я открыл созданный список
    Тогда вижу задачи: "Убрать комнату, Съесть борщ, Покормить хомяка, Приготовить борщ"

    Пусть я переименовал задачу №1 на "Съесть борщ со сметаной"
    И переместил задачу №1 на позицию №3
    Когда я открыл созданный список
    Тогда вижу задачи: "Убрать комнату, Покормить хомяка, Приготовить борщ, Съесть борщ со сметаной"
    И вижу завершённые задачи: ""

    Пусть я завершил задачу №0
    И завершил задачу №2
    Когда я открыл созданный список
    Тогда вижу задачи: "Убрать комнату, Покормить хомяка, Приготовить борщ, Съесть борщ со сметаной"
    И вижу завершённые задачи: "Убрать комнату, Приготовить борщ"

    Пусть я удалил задачу №1
    Когда я открыл созданный список
    Тогда вижу задачи: "Убрать комнату, Приготовить борщ, Съесть борщ со сметаной"
