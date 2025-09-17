import numpy as np
import statsmodels.api as sm
import seaborn as sns
import pandas as pd
import matplotlib.pyplot as plt
import os

pd.plotting.register_matplotlib_converters()
print("Setup Complete")


# Assuming data is in csv file
file_path = os.path.join(os.path.dirname(__file__), "expenses.csv")
df = pd.read_csv(file_path)

sns.lineplot(data=df, x="date", y="amount", hue="category")
plt.show()



sns.barplot(data=df, x="category", y="amount")


# Add title
plt.title('Amount Distribution by Category')
plt.show()