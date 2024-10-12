import { Queue } from "./Queue"

test('Queue can be used as FIFO', () => {
    const q = new Queue<string>();

    q.enqueue("red");
    q.enqueue("green");
    q.enqueue("blue");
    expect(q.size).toBe(3);

    expect(q.dequeue()).toBe("red");
    expect(q.size).toBe(2);

    expect(q.dequeue()).toBe("green");
    expect(q.size).toBe(1);

    expect(q.dequeue()).toBe("blue");
    expect(q.size).toBe(0);

    expect(q.dequeue()).toBe(undefined);
    expect(q.size).toBe(0);
})

test('Queue can increase capacity', () => {
    const q = new Queue<string>(2);
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(0);

    q.enqueue("red");
    q.enqueue("green");
    q.enqueue("blue");
    expect(q.capacity).toBe(4);
    expect(q.size).toBe(3);

    q.enqueue("white");
    q.enqueue("magenta");
    q.enqueue("cyan");
    q.enqueue("yellow");
    q.enqueue("black");
    expect(q.capacity).toBe(8);
    expect(q.size).toBe(8);

    expect(q.dequeue()).toBe("red");
    expect(q.dequeue()).toBe("green");
    expect(q.dequeue()).toBe("blue");
    expect(q.dequeue()).toBe("white");
    expect(q.dequeue()).toBe("magenta");
    expect(q.dequeue()).toBe("cyan");
    expect(q.dequeue()).toBe("yellow");
    expect(q.dequeue()).toBe("black");
})

test('Queue does not increase until size exceeds capacity', () => {
    const q = new Queue<string>(2);
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(0);

    q.enqueue("red");
    q.enqueue("green");
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(2);

    expect(q.dequeue()).toBe("red");
    q.enqueue("blue");
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(2);

    expect(q.dequeue()).toBe("green");
    expect(q.dequeue()).toBe("blue");
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(0);

    q.enqueue("white");
    q.enqueue("magenta");
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(2);

    expect(q.dequeue()).toBe("white");
    expect(q.dequeue()).toBe("magenta");
    expect(q.capacity).toBe(2);
    expect(q.size).toBe(0);
})