import sqlite3

conn = sqlite3.connect("expense_tracker.db")
cursor = conn.cursor()

cursor.execute("""
CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    salary INTEGER
);
""")

cursor.execute("""
CREATE TABLE IF NOT EXISTS transactions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    amount REAL NOT NULL,
    category TEXT NOT NULL,
    merchant TEXT,
    user_id INTEGER NOT NULL,
    date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id)
);
""")

conn.commit()
print("Database created")
conn.close()