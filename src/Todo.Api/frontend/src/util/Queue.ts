export class Queue<T> {
    // Циклический буфер для размещения элементов.
    private _values: T[];
    // Индекс первого элемента в буфере.
    private _start: number = 0;
    // Число элементов очереди.
    private _size: number = 0;

    constructor(capacity: number = 4) {
        this._values = new Array(capacity);
    }

    get size(): number {
        return this._size;
    }

    get capacity(): number {
        return this._values.length;
    }

    enqueue(value: T): void {
        if (this._size === this._values.length) {
            this.extend();
        }
        const index = (this._start + this._size) % this._values.length;
        this._values[index] = value;
        ++this._size;
    }

    dequeue(): T | undefined {
        if (this._size === 0) {
            return undefined;
        }
        const value: T = this._values[this._start];
        this._start = (this._start + 1) % this._values.length;
        --this._size;

        return value;
    }

    /**
     * Пересоздаёт буфер значений, увеличивая его объём вдвое.
     */
    private extend(): void {
        const newValues = new Array(2 * this._values.length);
        for (let toIndex = 0; toIndex < this._size; ++toIndex)
        {
            const fromIndex = (this._start + toIndex) % this._values.length;
            newValues[toIndex] = this._values[fromIndex];
        }

        this._start = 0;
        this._values = newValues;
    }
}