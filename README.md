
# ABP + DDD 实现方式示例项目（持续更新中...）

一个展示 ABP 框架与领域驱动设计（DDD）结合使用的 Demo 项目，持续更新中，欢迎交流与反馈。

---

## 🚀 启动方式

### 1. 配置启动服务

通过修改环境变量中的 `ServerMod` 来选择启动的服务：

- 可选值：`webapi`、`consumer` 等

### 2. 数据库初始化

```bash
dotnet ef migrations add InitUserDb \
  --context UserDbContext \
  --project .\DDD.Demo.Infrastructure\DDD.Demo.Infrastructure.csproj \
  --startup-project .\DDD.Demo\DDD.Demo.csproj

dotnet ef database update \
  --context UserDbContext \
  --project .\DDD.Demo.Infrastructure\DDD.Demo.Infrastructure.csproj \
  --startup-project .\DDD.Demo\DDD.Demo.csproj
```

### 3. 启动项目

在项目根目录下执行：

```bash
dotnet run
```

或在任意路径下直接指定项目文件：

```bash
dotnet run -p {你的本地路径}\DDD.Demo\DDD.Demo.csproj
```

---

## 🧩 项目功能一览

该 Demo 展示了以下关键功能的集成：

- ✅ 模型验证（Validation）
- ✅ 审计日志（Auditing）
- ✅ 后台任务（Background Jobs）
- ✅ 分布式锁（Distributed Lock）
- ✅ 本地事件总线（Event Bus）
- ✅ RabbitMQ 集成
- ✅ 支持通过环境变量动态启动不同服务（ServerMod）

---

## 📌 注意事项

> **本项目仅作为个人学习与总结使用，不建议直接照搬到生产环境中。**

绝大部分项目业务相对简单，直接采用复杂的架构设计可能会导致**过度设计**，影响开发效率与维护性。

---

## 🙌 欢迎交流

如果你有任何问题或优化建议：

- 欢迎提 Issue；
- 或直接加我微信交流 😊：

![微信二维码](https://github.com/user-attachments/assets/6e959968-8e48-4eaf-b667-da6f4c90744c)

