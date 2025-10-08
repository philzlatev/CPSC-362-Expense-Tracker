import sqlite3

conn = sqlite3.connect("expense_tracker.db")
cursor = conn.cursor()

cursor.execute("""
CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY, /*can add AUTOINCREMENT*/
    name TEXT NOT NULL,
    salary INTEGER
);
""")

cursor.execute("""
CREATE TABLE IF NOT EXISTS transactions (
  id INTEGER PRIMARY KEY, /*can add AUTOINCREMENT*/
  user_id INTEGER NOT NULL,
  amount REAL NOT NULL CHECK (amount >= 0),
  category TEXT NOT NULL CHECK (trim(category) <> ''),
  merchant TEXT,
  date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (user_id) REFERENCES users(id)
);
""")

conn.commit()
print("Database created")
conn.close()
