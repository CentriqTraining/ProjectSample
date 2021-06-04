import time
from locust import HttpUser, task, between

class QuickstartUser(HttpUser):
    wait_time = between(1, 2.5)

    @task
    def create_items(self):
        for item_id in range(10):
            emp = between(1, 3)
            hours = between(1,15)
            project = between(1,54)
            self.client.post("/Home/Create", json={"Employee": 1, "Project": 5, "Hours": 4 }, verify=False)
