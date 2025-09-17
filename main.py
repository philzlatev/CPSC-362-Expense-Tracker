import numpy as np
import statsmodels.api as sm
import seaborn as sns
import pandas as pd
import matplotlib.pyplot as plt

pd.plotting.register_matplotlib_converters()
print("Setup Complete")


# Assuming data is in csv file
file_path = "/Users/suhailkhaled/CPSC-362-Expense-Tracker/expenses.csv"

df = pd.read_csv(file_path)

sns.lineplot(data=df, x="date", y="amount", hue="category")
plt.show()